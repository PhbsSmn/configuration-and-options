using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<MyOptions>().BindConfiguration("MyOptions");

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/options", (IOptions<MyOptions> options, IOptionsMonitor<MyOptions> optionsMonitor, IOptionsSnapshot<MyOptions> optionsSnapshot) =>
{
    var configOptions = options.Value;
    var configOptionsMonitor = optionsMonitor.CurrentValue;
    var configOptionsSnapshot = optionsSnapshot.Value;

    return new
    {
        Options = new { configOptions.Id, configOptions.Setting },
        OptionsMonitor = new { configOptionsMonitor.Id, configOptionsMonitor.Setting },
        OptionsSnapshot = new { configOptionsSnapshot.Id, configOptionsSnapshot.Setting }
    };
});
app.Run();

public class MyOptions
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Setting { get; set; } = string.Empty;
}