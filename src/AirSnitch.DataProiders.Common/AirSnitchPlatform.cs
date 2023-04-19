using AirSnitch.SDK;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AirSnitch.DataProiders.Common;

public class AirSnitchPlatform
{
	public async Task SubmitMeasurement(DataPoint dataPoint, SqsConfig config) {
		var awsCreds = new BasicAWSCredentials(config.AccessKey, config.SecretKey);
		var amazonSqsConfig = new AmazonSQSConfig();
		var client = new AmazonSQSClient(awsCreds, amazonSqsConfig);
		var messageRequest = new SendMessageRequest(queueUrl: config.QueueUrl, messageBody: dataPoint.Serialize());
		messageRequest.MessageGroupId = "Measrements";
		await client.SendMessageAsync(messageRequest);
	}
}
