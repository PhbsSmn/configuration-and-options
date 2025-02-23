namespace ConfigureNonDiClient.Client
{
    public interface INonDiClient
    {
        string GetKey();
    }
    public static class NonDiClientFactory
    {
        public static INonDiClient CreateClient(NonDiClientSettings settings)
        {
            return new NonDiClient(settings);
        }

        private class NonDiClient : INonDiClient
        {
            private readonly NonDiClientSettings _settings;
            public NonDiClient(NonDiClientSettings settings)
            {
                _settings = settings;
            }
            public string GetKey()
            {
                return _settings.Key;
            }
        }
    }

    public class NonDiClientSettings
    {
        public required string Key { get; set; }
    }
}
