using CustomConfigurationProvider.Configuration.Aws;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAwsSecretsManager("Test");

var app = builder.Build();
app.UseHttpsRedirection();

// TODO map get for the configuration

app.Run();
