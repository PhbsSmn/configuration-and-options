using CustomConfigurationProvider.Configuration.Aws;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddAwsSecretsManager("Test");

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/appsettings", (IConfiguration configuration) =>
{
    return configuration.GetSection("appsettings").GetChildren().Select(x => $"{x.Key}={x.Value}").ToArray();
});

app.Run();