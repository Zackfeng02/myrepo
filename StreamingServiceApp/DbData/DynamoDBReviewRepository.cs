using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
        private const string TableName = "Reviews";

        public DynamoDBReviewRepository()
        {
            _dynamoDbHelper = new DynamoDBHelper();
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        {
            var keyConditionExpression = "MovieId = :movieId";
            var expressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":movieId", new AttributeValue { N = movieId.ToString() } }
            };

            var items = await _dynamoDbHelper.QueryTable(TableName, keyConditionExpression, expressionAttributeValues);
            return items.Select(DynamoDBItemToReview);
        }

        public async Task SaveReviewAsync(Review review)
        {
            var item = ReviewToDynamoDBItem(review);
            await _dynamoDbHelper.PutItem(TableName, item);
        }

        private Dictionary<string, AttributeValue> ReviewToDynamoDBItem(Review review)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "ReviewID", new AttributeValue { S = review.ReviewID } },
                { "Title", new AttributeValue { S = review.Title } },
                { "ReviewDescription", new AttributeValue { S = review.ReviewDescription } },
                { "MovieId", new AttributeValue { N = review.MovieId.ToString() } },
                { "MovieRating", new AttributeValue { N = review.MovieRating.ToString() } },
                { "UserEmail", new AttributeValue { S = review.UserEmail } }
                // ... add other properties as needed
            };
        }

        private Review DynamoDBItemToReview(Dictionary<string, AttributeValue> item)
        {
            return new Review
            {
                ReviewID = item["ReviewID"].S,
                Title = item["Title"].S,
                ReviewDescription = item["ReviewDescription"].S,
                MovieId = int.Parse(item["MovieId"].N),
                MovieRating = int.Parse(item["MovieRating"].N),
                UserEmail = item["UserEmail"].S
                // ... add other properties as needed
            };
        }
    }
}
