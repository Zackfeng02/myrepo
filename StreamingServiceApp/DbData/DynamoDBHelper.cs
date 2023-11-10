using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace StreamingServiceApp.DbData
{
    public class DynamoDBHelper
    {
        private readonly AmazonDynamoDBClient _dynamoDbClient;

        public DynamoDBHelper()
        {
            var connection = new Connection();
            _dynamoDbClient = connection.Connect();
        }

        public async Task<List<Dictionary<string, AttributeValue>>> ScanTable(string tableName, string filterExpression, Dictionary<string, AttributeValue> expressionAttributeValues)
        {
            try
            {
                var request = new ScanRequest
                {
                    TableName = tableName,
                    FilterExpression = filterExpression,
                    ExpressionAttributeValues = expressionAttributeValues
                };
                var response = await _dynamoDbClient.ScanAsync(request);
                return response.Items;
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
                return new List<Dictionary<string, AttributeValue>>();
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
                return new List<Dictionary<string, AttributeValue>>();
            }
        }


        public async Task<Dictionary<string, AttributeValue>> GetItem(string tableName, Dictionary<string, AttributeValue> key)
        {
            try
            {
                var request = new GetItemRequest { TableName = tableName, Key = key };
                var response = await _dynamoDbClient.GetItemAsync(request);
                return response.Item;
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
                return new Dictionary<string, AttributeValue>();
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
                return new Dictionary<string, AttributeValue>();
            }

        }

        public async Task PutItem(string tableName, Dictionary<string, AttributeValue> item)
        {
            try
            {
                var request = new PutItemRequest { TableName = tableName, Item = item };
                var response = await _dynamoDbClient.PutItemAsync(request);
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
            }


        }

        public async Task UpdateItem(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates)
        {
            try
            {
                var request = new UpdateItemRequest { TableName = tableName, Key = key, AttributeUpdates = attributeUpdates };
                await _dynamoDbClient.UpdateItemAsync(request);
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
            }

        }

        public async Task DeleteItem(string tableName, Dictionary<string, AttributeValue> key)
        {
            try
            {
                var request = new DeleteItemRequest { TableName = tableName, Key = key };
                await _dynamoDbClient.DeleteItemAsync(request);
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
            }
        }

        public async Task<List<Dictionary<string, AttributeValue>>> QueryTable(string tableName, string keyConditionExpression, Dictionary<string, AttributeValue> expressionAttributeValues)
        {
            try
            {
                var request = new QueryRequest { TableName = tableName, KeyConditionExpression = keyConditionExpression, ExpressionAttributeValues = expressionAttributeValues };
                var response = await _dynamoDbClient.QueryAsync(request);
                return response.Items;
            }
            catch (ProvisionedThroughputExceededException)
            {
                Console.WriteLine("Throughput limit exceeded. Consider increasing your provisioned throughput.");
                return new List<Dictionary<string, AttributeValue>>();
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine($"Table {tableName} not found.");
                return new List<Dictionary<string, AttributeValue>>();
            }
        }

        public async Task<List<Dictionary<string, AttributeValue>>> QueryIndex(
            string tableName,
            string indexName,
            string keyConditionExpression,
            Dictionary<string, AttributeValue> expressionAttributeValues,
            string filterExpression = null) // Optional parameter for filter expression
        {
            var request = new QueryRequest
            {
                TableName = tableName,
                IndexName = indexName,
                KeyConditionExpression = keyConditionExpression,
                ExpressionAttributeValues = expressionAttributeValues,
                FilterExpression = filterExpression // Use the filter expression if provided
            };

            var response = await _dynamoDbClient.QueryAsync(request);
            return response.Items;
        }

        public async Task<List<Dictionary<string, AttributeValue>>> QueryAsync(
        string tableName,
        string indexName,
        string keyConditionExpression,
        Dictionary<string, AttributeValue> expressionAttributeValues,
        string filterExpression = null) // Optional parameter for filter expression
        {
            var request = new QueryRequest
            {
                TableName = tableName,
                IndexName = indexName,
                KeyConditionExpression = keyConditionExpression,
                ExpressionAttributeValues = expressionAttributeValues,
                FilterExpression = filterExpression // Use the filter expression if provided
            };

            try
            {
                var response = await _dynamoDbClient.QueryAsync(request);
                return response.Items;
            }
            catch (Exception e)
            {
                // Handle any exceptions here
                Console.WriteLine(e.Message);
                return new List<Dictionary<string, AttributeValue>>();
            }
        }


    }
}
