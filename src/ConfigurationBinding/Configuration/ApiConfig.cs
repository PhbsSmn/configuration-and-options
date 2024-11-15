namespace ConfigurationBinding.Configuration;

internal class ApiConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public string? ApiKey { get; set; }
    public int Timeout { get; set; }
    public Guid? ClientId { get; set; }
}
