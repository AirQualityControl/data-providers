using AirSnitch.SDK;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AirSnitch.DataProiders.Common;

public class AirSnitchPlatform
{
	public async Task SubmitMeasurement(DataPoint dataPoint)
	{
#if DEBUG
		await Task.Delay(300);
#else
		var awsCreds = new BasicAWSCredentials("AKIA3ZQ3P5GAMNV53MYK", "DORBFUoTVzqM9zzhmR+xQlG42DgxDnKrJwQL4apT");

		var amazonSqsConfig = new AmazonSQSConfig();
		var client = new AmazonSQSClient(awsCreds, amazonSqsConfig);
		await client.SendMessageAsync(new SendMessageRequest(queueUrl: "https://sqs.eu-west-3.amazonaws.com/810732743040/SensorsMeasurements.fifo", messageBody: dataPoint.Serialize()));
#endif
		}
}
