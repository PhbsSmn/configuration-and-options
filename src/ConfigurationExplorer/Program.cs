var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
    { new("AppSettings:Setting7", "MemoryValue7") });

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/configuration", (IConfiguration configuration) =>
     configuration
        .AsEnumerable()
        .OrderBy(kvp => kvp.Key)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
);

app.MapGet("/configuration-providers", ParseConfigurationPerProvider);

static IEnumerable<string?> ParseConfigurationPerProvider(IConfiguration configuration)
{
    if (configuration is IConfigurationRoot configurationRoot)
    {
        foreach (var provider in configurationRoot.Providers)
        {
            yield return provider.ToString();
        }
    }
}

app.Run();
