using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Mvc;
using StreamingServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamingServiceApp.DbData
{
    public class DynamoDBReviewRepository : IReviewRepository
    {
        private readonly DynamoDBHelper _dynamoDbHelper;
        private const string TableName = "StreamingServiceData"; // Adjusted to the single table name

        public DynamoDBReviewRepository()
        {
            _dynamoDbHelper = new DynamoDBHelper();
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        {
            var keyConditionExpression = "PK = :pk AND begins_with(SK, :sk)";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = $"MOVIE#{movieId}" } },
                { ":sk", new AttributeValue { S = "REVIEW#" } }
            };

            var items = await _dynamoDbHelper.QueryTable(TableName, keyConditionExpression, expressionAttributeValues);
            return items.Select(DynamoDBItemToReview);
        }

        public async Task SaveReviewAsync(Review review)
        {
            if (review.CreatedAt == default(DateTime))
            {
                review.CreatedAt = DateTime.UtcNow;
            }
            var item = ReviewToDynamoDBItem(review);
            await _dynamoDbHelper.PutItem(TableName, item);
        }

        private Dictionary<string, AttributeValue> ReviewToDynamoDBItem(Review review)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{review.MovieId}" } },
                { "SK", new AttributeValue { S = $"REVIEW#{review.ReviewID}" } },
                { "ReviewID", new AttributeValue { S = review.ReviewID } },
                { "Title", new AttributeValue { S = review.Title } },
                { "ReviewDescription", new AttributeValue { S = review.ReviewDescription } },
                { "MovieRating", new AttributeValue { N = review.MovieRating.ToString() } },
                { "UserEmail", new AttributeValue { S = review.UserEmail } },
                { "MovieId", new AttributeValue { N = review.MovieId.ToString() } },
                { "CreatedAt", new AttributeValue { S = review.CreatedAt.ToString("o") } }
                
            };
        }

        private Review DynamoDBItemToReview(Dictionary<string, AttributeValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var review = new Review();

            AttributeValue value;
            if (item.TryGetValue("ReviewID", out value))
            {
                review.ReviewID = value.S;
            }
            else
            {
                // Handle the absence of ReviewID appropriately
                throw new KeyNotFoundException("The 'ReviewID' key was not found in the item dictionary.");
            }

            if (item.TryGetValue("Title", out value))
            {
                review.Title = value.S;
            }

            if (item.TryGetValue("ReviewDescription", out value))
            {
                review.ReviewDescription = value.S;
            }

            if (item.TryGetValue("MovieRating", out value))
            {
                review.MovieRating = int.Parse(value.N);
            }

            if (item.TryGetValue("UserEmail", out value))
            {
                review.UserEmail = value.S;
            }

            if (item.TryGetValue("MovieId", out value))
            {
                review.MovieId = int.Parse(value.N);
            }

            if (item.TryGetValue("CreatedAt", out value))
            {
                review.CreatedAt = DateTime.Parse(value.S);
            }

            return review;
        }

        public async Task<Review> GetReviewByIdAsync(string reviewId)
        {
            string movieId = await GetMovieIdForReview(reviewId);

            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{movieId}" } },
                { "SK", new AttributeValue { S = $"REVIEW#{reviewId}" } }
            };

            var item = await _dynamoDbHelper.GetItem(TableName, key);
            return item == null ? null : DynamoDBItemToReview(item);
        }

        public async Task UpdateReviewAsync(Review review)
        {

            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{review.MovieId}" } },
                { "SK", new AttributeValue { S = $"REVIEW#{review.ReviewID}" } }
            };

            var attributeUpdates = new Dictionary<string, AttributeValueUpdate>
            {
                { "Title", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = review.Title } } },
                { "ReviewDescription", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = review.ReviewDescription } } },
                { "MovieRating", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = review.MovieRating.ToString() } } },
                { "CreatedAt", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = review.CreatedAt.ToString("o") } } },
                { "MovieId", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { N = review.MovieId.ToString() } } },
                { "UserEmail", new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = review.UserEmail } } },
            };

            try
            {
                await _dynamoDbHelper.UpdateItem(TableName, key, attributeUpdates);
                
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                
            }
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            string movieId = await GetMovieIdForReview(reviewId);

            var key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = $"MOVIE#{movieId}" } },
                { "SK", new AttributeValue { S = $"REVIEW#{reviewId}" } }
            };

            await _dynamoDbHelper.DeleteItem(TableName, key);
        }

        public async Task<string> GetMovieIdForReview(string reviewId)
        {
            var helper = new DynamoDBHelper(); // Assuming you have an instance of DynamoDBHelper
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":v_ReviewID", new AttributeValue { S = reviewId }}
            };

            var items = await helper.QueryAsync(
                tableName: "StreamingServiceData",
                indexName: "ReviewID-index", // Replace with your GSI name
                keyConditionExpression: "ReviewID = :v_ReviewID",
                expressionAttributeValues: expressionAttributeValues
            );

            if (items.Count > 0)
            {
                // Assuming 'MovieId' is stored as a string in DynamoDB
                return items[0]["MovieId"].N;
            }

            return null;
        }


    }
}
