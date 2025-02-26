using ConfigureNonDiClient.Client;
using Microsoft.Extensions.Options;

var defaultClient = NonDiClientFactory.CreateClient(new NonDiClientSettings { Key = "DefaultClient" });
var builder = WebApplication.CreateBuilder(args);
#region Load it as a DI service
builder.Services.AddSingleton(sp =>
{
    var nonDiClientOptions = sp.GetRequiredService<IOptions<NonDiClientOptions>>().Value;
    return NonDiClientFactory.CreateClient(new NonDiClientSettings() { Key = nonDiClientOptions.Key });
});
builder.Services.AddOptions<NonDiClientOptions>().BindConfiguration(NonDiClientOptions.CONFIGURATION_SECTION_PATH).ValidateDataAnnotations().ValidateOnStart();
#endregion
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/NonDiClient", () => defaultClient.GetKey());
app.MapGet("/DiClient", (INonDiClient client) => client.GetKey());

app.Run();