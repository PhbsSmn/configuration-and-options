namespace CustomConfigurationProvider.Configuration.Aws;

internal static class AwsSecretsManagerConfigurationExtensions
{
    public static IConfigurationBuilder AddAwsSecretsManager(this IConfigurationBuilder builder, string secretId, int? reloadOnFailureDelay = null)
    {
        builder.Add(new AwsSecretsManagerConfigurationSource(secretId, reloadOnFailureDelay));
        return builder;
    }
}
