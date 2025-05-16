using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration.Json;
using System.Text;

namespace CustomConfigurationProvider.Configuration.Aws;

internal class AwsSecretsManagerConfigurationProvider : JsonConfigurationProvider
{
    private readonly string _secretId;
    private readonly bool _reloadOnFailureActive;
    private readonly int _reloadOnFailureDelay;

    public AwsSecretsManagerConfigurationProvider(string secretId, int? reloadOnFailureDelay) : base(new JsonConfigurationSource())
    {
        _secretId = secretId;

        if (reloadOnFailureDelay is > 0)
        {
            _reloadOnFailureActive = true;
            _reloadOnFailureDelay = reloadOnFailureDelay.Value;
        }
    }

    // Load the configuration data from AWS Secrets Manager
    public override void Load()
    {
        using (var stream = GetSecret())
        {
            // Load the configuration data from the stream into the json configuration provider
            base.Load(stream);
        }
    }

    private MemoryStream GetSecret()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = _secretId
        };

        try
        {
            using (var client = new AmazonSecretsManagerClient())
            {
                // Get the secret value from AWS Secrets Manager
                // We can't use async/await here because the Load method is synchronous
                var response = client.GetSecretValueAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

                var secretString = response.SecretString != null
                    ? GetSecretString(response)
                    : GetSecretBinary(response);

                return secretString;
            }
        }
        catch (Exception e)
        {
            if (_reloadOnFailureActive)
            {
                Task.Run(Reload);
            }

            var formatException = new FormatException("Error loading secrets from AWS Secrets Manager", e);
            Console.WriteLine(formatException);
            return new MemoryStream("{}"u8.ToArray());
        }
    }

    private async void Reload()
    {
        await Task.Delay(_reloadOnFailureDelay).ConfigureAwait(false);
        Load();
        OnReload();
    }

    private static MemoryStream GetSecretString(GetSecretValueResponse response)
        => new(Encoding.UTF8.GetBytes(response.SecretString));

    private static MemoryStream GetSecretBinary(GetSecretValueResponse response)
        => response.SecretBinary;
}