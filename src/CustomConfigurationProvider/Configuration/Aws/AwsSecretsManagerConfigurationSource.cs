namespace CustomConfigurationProvider.Configuration.Aws;

internal class AwsSecretsManagerConfigurationSource : IConfigurationSource
{
    private readonly string _secretId;
    private readonly int? _reloadOnFailureDelay;

    public AwsSecretsManagerConfigurationSource(string secretId, int? reloadOnFailureDelay = null)
    {
        _secretId = secretId;
        _reloadOnFailureDelay = reloadOnFailureDelay;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new AwsSecretsManagerConfigurationProvider(_secretId, _reloadOnFailureDelay);
    }
}
