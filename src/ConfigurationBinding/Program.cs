
using ConfigurationBinding.Configuration;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/config/{option:int}", (int option, IConfiguration configuration) => option switch
{
    1 => configuration.ConfigureApiConfigBasic(),
    2 => configuration.ConfigureApiConfigWithValidation(),
    3 => configuration.ConfigureApiConfigWithGetValue(),
    4 => configuration.ConfigureApiConfigBind(),
    _ => throw new NotImplementedException("Unknown option")
});

app.Run();

internal static class ConfigurationExtensions
{
    public static ApiConfig ConfigureApiConfigBasic(this IConfiguration configuration)
    {
        return new ApiConfig
        {
            BaseUrl = configuration["ApiConfig:BaseUrl"]!,
            ApiKey = configuration["ApiConfig:ApiKey"],
            Timeout = Convert.ToInt32(configuration["ApiConfig:Timeout"]),
            ClientId = configuration["ApiConfig:ClientId"] != null ? new Guid(configuration["ApiConfig:ClientId"]!) : null
        };
    }

    public static ApiConfig ConfigureApiConfigWithValidation(this IConfiguration configuration)
    {
        var baseUrl = configuration["ApiConfig:BaseUrl"];
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        if (baseUrl.Length > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(baseUrl), "Url is too long");
        }

        var apiKey = configuration["ApiConfig:ApiKey"];

        var rawTimeout = configuration["ApiConfig:Timeout"];
        ArgumentException.ThrowIfNullOrWhiteSpace(rawTimeout);

        if (!int.TryParse(rawTimeout, out var timeout))
        {
            throw new ArgumentException("Timeout is mandatory", nameof(timeout));
        }
        if (timeout < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Id must be greater than 0");
        }

        var rawClientId = configuration["ApiConfig:ClientId"];

        if (string.IsNullOrWhiteSpace(rawClientId))
        {
            return new ApiConfig
            {
                Timeout = timeout,
                BaseUrl = baseUrl,
                ApiKey = apiKey
            };
        }

        if (!Guid.TryParse(rawClientId, out var clientId))
        {
            throw new FormatException($"{nameof(clientId)} format '{rawClientId}' is invalid");
        }
        return new ApiConfig
        {
            Timeout = timeout,
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            ClientId = clientId
        };
    }

    public static ApiConfig ConfigureApiConfigWithGetValue(this IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("ApiConfig:BaseUrl");
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        if (baseUrl.Length > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(baseUrl), "Url is too long");
        }

        var apiKey = configuration.GetValue<string?>("ApiConfig:ApiKey");

        var timeout = configuration.GetValue<int?>("ApiConfig:Timeout", null);
        if (timeout == null)
        {
            throw new ArgumentException("Timeout is mandatory", nameof(timeout));
        }
        if (timeout.Value < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Id must be greater than 0");
        }

        var rawClientId = configuration["ApiConfig:ClientId"];

        if (string.IsNullOrWhiteSpace(rawClientId))
        {
            return new ApiConfig
            {
                Timeout = timeout.Value,
                BaseUrl = baseUrl,
                ApiKey = apiKey
            };
        }

        if (!Guid.TryParse(rawClientId, out var clientId))
        {
            throw new FormatException($"{nameof(clientId)} format '{rawClientId}' is invalid");
        }
        return new ApiConfig
        {
            Timeout = timeout.Value,
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            ClientId = clientId
        };
    }

    public static ApiConfig ConfigureApiConfigBind(this IConfiguration configuration)
    {
        var apiConfig = new ApiConfig { BaseUrl = string.Empty, Timeout = 0 };
        configuration.GetSection("ApiConfig").Bind(apiConfig);
        configuration.Bind("ApiConfig", apiConfig);

        ArgumentException.ThrowIfNullOrWhiteSpace(apiConfig.BaseUrl);
        if (apiConfig.BaseUrl.Length > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(apiConfig.BaseUrl), "Url is too long");
        }

        // Required check is lost here, could be fixed when using int?
        if (apiConfig.Timeout < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(apiConfig.Timeout), "Id must be greater than 0");
        }

        return apiConfig;
    }
}