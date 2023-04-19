using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace AirSnitch.DataProiders.Common;

public class SqsConfig
{
	public string AccessKey { get; private set; }
	public string SecretKey { get; private set; }
	public string QueueUrl { get; private set; }

	private SqsConfig()
	{}

	private async Task<SqsConfig> InitAsync() {
		using (var client = new AmazonSimpleSystemsManagementClient()) {
			var request = new GetParametersRequest {
				Names = new() { "AirSnitchAccessKey", "AirSnitchSecretKey", "SqsMeasurementQueueUrl" },
				WithDecryption = true
			};
			var response = await client.GetParametersAsync(request);
			AccessKey = response.Parameters.FirstOrDefault(item => item.Name == "AirSnitchAccessKey")?.Value;
			SecretKey = response.Parameters.FirstOrDefault(item => item.Name == "AirSnitchSecretKey")?.Value;
			QueueUrl = response.Parameters.FirstOrDefault(item => item.Name == "SqsMeasurementQueueUrl")?.Value;
		}
		return this;
	}

	public static Task<SqsConfig> CreateAsync() {
		var result = new SqsConfig();
		return result.InitAsync();
	}
}
