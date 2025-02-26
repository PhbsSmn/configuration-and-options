using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<SecretOptions>("A")
    .BindConfiguration($"{SecretOptions.CONFIGURATION_SECTION_PATH}:A")
    .ValidateDataAnnotations()
    .ValidateOnStart();
//builder.Services.AddOptions<SecretOptions>("B")
//    .BindConfiguration($"{SecretOptions.CONFIGURATION_SECTION_PATH}:B")
//    .ValidateDataAnnotations()
//    .ValidateOnStart();
//builder.Services.AddOptions<SecretOptions>("C")
//    .BindConfiguration($"{SecretOptions.CONFIGURATION_SECTION_PATH}:C")
//    .ValidateDataAnnotations()
//    .ValidateOnStart();
builder.Services.AddOptions<SecretOptions>("D")
    .BindConfiguration($"{SecretOptions.CONFIGURATION_SECTION_PATH}:D")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// builder.Services
/* // 2
.PostConfigureAll((SecretOptions o) =>
{
o.Enabled ??= !string.IsNullOrWhiteSpace(o.PublicKey) && !string.IsNullOrWhiteSpace(o.PrivateKey);
})
*/
/* // 1
.PostConfigureAll((SecretOptions o) =>
{
if (o.Enabled.HasValue && o.Enabled.Value)
{
    return;
}

o.PublicKey ??= "N/A";
o.PrivateKey ??= "N/A";
})*/
;

var app = builder.Build();
app.UseHttpsRedirection();
app.MapGet("/secret/{flavor}", (IOptionsMonitor<SecretOptions> optionsMonitor, string flavor) =>
{
    var options = optionsMonitor.Get(flavor);
    if (!options.Enabled.HasValue)
    {
        return "Secret is not configured";
    }

    return options.Enabled.Value
        ? $"Public Key: {options.PublicKey}, Private Key: {options.PrivateKey}"
        : "Secret is disabled";
});
app.Run();

public class SecretOptions
{
    public const string CONFIGURATION_SECTION_PATH = "Secret";

    [Required]
    public bool? Enabled { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? PublicKey { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? PrivateKey { get; set; }
}