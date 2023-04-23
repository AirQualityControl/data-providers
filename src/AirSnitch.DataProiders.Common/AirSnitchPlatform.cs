using AirSnitch.SDK;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AirSnitch.DataProiders.Common;

public class AirSnitchPlatform
{

	private static readonly Guid MessageGroupId = Guid.Parse("012f6009-4e6e-455f-9b9b-98f2dc9a6155");
	public static async Task SubmitMeasurement(DataPoint dataPoint, SqsConfig config) {
#if DEBUG
		await Task.Delay(400);
#else
		var awsCreds = new BasicAWSCredentials(config.AccessKey, config.SecretKey);
		var amazonSqsConfig = new AmazonSQSConfig();
		var client = new AmazonSQSClient(awsCreds, amazonSqsConfig);
		var messageRequest = new SendMessageRequest(queueUrl: config.QueueUrl, messageBody: dataPoint.Serialize());
		messageRequest.MessageGroupId = MessageGroupId.ToString();
        messageRequest.MessageDeduplicationId = Guid.NewGuid().ToString(); //Skip message deduplication. Treat each message as a unique one
        await client.SendMessageAsync(messageRequest);
#endif
    }
}
