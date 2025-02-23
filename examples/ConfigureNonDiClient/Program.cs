using ConfigureNonDiClient.Client;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var defaultClient = NonDiClientFactory.CreateClient(new NonDiClientSettings { Key = "DefaultClient" });
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(sp =>
{
    var nonDiClientOptions = sp.GetRequiredService<IOptions<NonDiClientOptions>>().Value;
    return NonDiClientFactory.CreateClient(new NonDiClientSettings() { Key = nonDiClientOptions.Key });
});
builder.Services.AddOptions<NonDiClientOptions>().BindConfiguration(NonDiClientOptions.CONFIGURATION_SECTION_PATH);

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/NonDiClient", () => defaultClient.GetKey());
app.MapGet("/DiClient", (INonDiClient client) => client.GetKey());

app.Run();

public class NonDiClientOptions
{
    public const string CONFIGURATION_SECTION_PATH = "NonDiClient";

    [Required(AllowEmptyStrings = false)]
    public string Key { get; set; }
}