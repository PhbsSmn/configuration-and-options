using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    file sealed class InterceptsLocationAttribute : Attribute
    {
        public InterceptsLocationAttribute(string filePath, int line, int column)
        {
        }
    }
}

namespace ConfigurationProvider.Mock
{
    public static class AwsExtensions
    {
        [System.Runtime.CompilerServices.InterceptsLocation(@"C:\Presentations\configuration-and-options\src\CustomConfigurationProvider\Configuration\Aws\AwsSecretsManagerConfigurationProvider.cs", 48, 39)]
        internal static Task<GetSecretValueResponse> GetSecretValueAsync(this AmazonSecretsManagerClient client,
            GetSecretValueRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new GetSecretValueResponse()
            {
                SecretString = """
                {
                    "AppSettings": {
                        "Setting": "SecretsManagerValue"
                    }
                }
                """
            });
        }
    }
}
