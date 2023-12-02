using AirSnitch.SDK;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AirSnitch.DataProiders.Common;

public class AirSnitchPlatform
{

	public static async Task SubmitMeasurement(DataPoint dataPoint, SqsConfig config) {
#if DEBUG
		await Task.Delay(400);
#else
		var awsCreds = new BasicAWSCredentials(config.AccessKey, config.SecretKey);
		var amazonSqsConfig = new AmazonSQSConfig();
		var client = new AmazonSQSClient(awsCreds, amazonSqsConfig);
		var messageRequest = new SendMessageRequest(queueUrl: config.QueueUrl, messageBody: dataPoint.Serialize());
        await client.SendMessageAsync(messageRequest);
#endif
    }
}
