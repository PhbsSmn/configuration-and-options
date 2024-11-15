using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using OptionConfigure.Options;
using OptionConfigure.Serialization;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IValidateOptions<MyOptions>, ValidateMyOptions>();
builder.Services.AddSingleton<IValidateOptions<FinalOptions>, ValidateFinalOptions>();
builder.Services.AddOptions<JsonOptions>().Configure(o => o.SerializerOptions.TypeInfoResolver = JsonTypeInfoResolver.Combine(OptionsSerializerContext.Default));

#region Configure MyOptions
builder.Services.ConfigureAll<MyOptions>(o =>
{
    o.Order.Add("ServiceCollection, ConfigureAll 1");
    Console.WriteLine($"Id: {o.Id}, Services ConfigureAll 1");
});

builder.Services
    .AddOptionsWithValidateOnStart<MyOptions>("A")
    .BindConfiguration("MyOptions:A")
    .Configure(o =>
    {
        o.Order.Add("ObjectBuilder, Configure A");
        Console.WriteLine($"Id: {o.Id}, OptionBuilder Configure A");
    })
    .PostConfigure(o =>
    {
        o.Order.Add("ObjectBuilder, PostConfigure A");
        Console.WriteLine($"OB: {o.Id}, OptionBuilder PostConfigure A");
    });
builder.Services
    .AddOptionsWithValidateOnStart<MyOptions>("B")
    .BindConfiguration("MyOptions:B");
builder.Services
    .AddOptionsWithValidateOnStart<MyOptions>("C")
    .BindConfiguration("MyOptions:C");


builder.Services.Configure<MyOptions>(o =>
{
    o.Order.Add("ServiceCollection, Configure Unnamed 1");
    Console.WriteLine($"Id: {o.Id}, Services Configure unspecified before A");
});
builder.Services.PostConfigure<MyOptions>(o =>
{
    o.Order.Add("ServiceCollection, PostConfigure Unnamed 1");
    Console.WriteLine($"Id: {o.Id}, Services PostConfigure unspecified before A");
});
builder.Services.ConfigureAll<MyOptions>(o =>
{
    o.Order.Add("ServiceCollection, ConfigureAll 2");
    Console.WriteLine($"Id: {o.Id}, Services ConfigureAll 2");
});
builder.Services.Configure<MyOptions>("A", o =>
{
    o.Order.Add("ServiceCollection, Configure A"); ;
    Console.WriteLine($"Id: {o.Id}, Services Configure A");
});
builder.Services.PostConfigure<MyOptions>("A", o =>
{
    o.Order.Add("ServiceCollection, PostConfigure A");
    Console.WriteLine($"Id: {o.Id}, Services PostConfigure A");
});
builder.Services.PostConfigureAll<MyOptions>(o =>
{
    o.Order.Add("ServiceCollection, PostConfigureAll");
    Console.WriteLine($"Id: {o.Id}, Services PostConfigureAll");
});
builder.Services.PostConfigure<MyOptions>(o =>
{
    o.Order.Add("ObjectBuilder, PostConfigure Unnamed 2");
    Console.WriteLine($"Id: {o.Id}, Services PostConfigure unspecified after A");
});
builder.Services.Configure<MyOptions>(o =>
{
    o.Order.Add("ObjectBuilder, Configure Unnamed 2");
    Console.WriteLine($"Id: {o.Id}, Services Configure unspecified after A");
});
#endregion

#region Configure Final
builder.Services.AddOptions<FinalOptions>()
    .BindConfiguration(FinalOptions.CONFIGURATION_SECTION_PATH)
    .Configure((FinalOptions finalOptions, IOptionsMonitor<MyOptions> optionsMonitor) =>
    {
        finalOptions.Options.Add(optionsMonitor.Get("A"));
        finalOptions.Options.Add(optionsMonitor.Get("B"));
        finalOptions.Options.Add(optionsMonitor.Get("C"));
    });

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/Configure/OrderMonitor",
    (IOptionsMonitor<MyOptions> optionsMonitor)
        => new Dictionary<string, MyOptions>
        {
            { nameof(optionsMonitor) + "->A", optionsMonitor.Get("A")},
            { nameof(optionsMonitor) + "->C", optionsMonitor.Get("C")},
        });
app.MapGet("/Configure/OrderSnapshot",
    (IOptionsSnapshot<MyOptions> optionsSnapshot)
        => new Dictionary<string, MyOptions>
        {
            { nameof(optionsSnapshot) + "->A", optionsSnapshot.Get("A")},
            { nameof(optionsSnapshot) + "->B", optionsSnapshot.Get("B")},
        });

app.MapGet("/Configure/Final/Bad", (IOptionsMonitor<FinalOptions> options) => options.Get("bad"));
app.Map("/Configure/Final/Good", (IOptions<FinalOptions> options) => options.Value);
app.Run();