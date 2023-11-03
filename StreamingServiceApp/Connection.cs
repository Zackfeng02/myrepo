using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;

namespace StreamingServiceApp
{
    class Connection
    {
        public AmazonDynamoDBClient Connect()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccessKeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("SecretAccessKey").Value;
            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            return client;
        }

        public AmazonS3Client ConnectS3()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccessKeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("SecretAccessKey").Value;

            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey, Amazon.RegionEndpoint.USEast1);
            return client;
        }


    }
}
