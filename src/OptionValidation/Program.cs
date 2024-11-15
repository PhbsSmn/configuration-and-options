using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization.Metadata;
using OptionValidation.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<MyOptions>().BindConfiguration("MyOptions")
    //.ValidateDataAnnotations()
    //.ValidateOnStart()
    ;
//builder.Services.AddOptionsWithValidateOnStart<MyOptions>().BindConfiguration("MyOptions")//.ValidateDataAnnotations();

//builder.Services.AddSingleton<IValidateOptions<MyOptions>, ValidateMyOptions>();
/*
builder.Services.AddOptions<JsonOptions>().Configure(o =>
    o.SerializerOptions.TypeInfoResolver = JsonTypeInfoResolver.Combine(OptionValidation.Serialization.OptionsSerializerContext.Default));
*/

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/validate1", (IOptions<MyOptions> options) => options.Value);
app.MapGet("/validate2", (IOptionsMonitor<MyOptions> options) => options.CurrentValue);
app.MapGet("/validate3", (IOptionsSnapshot<MyOptions> options) => options.Value);

app.MapGet("/validate4", (IOptionsMonitor<MyOptions> options) => options.Get("X"));
app.MapGet("/validate5", (IOptionsMonitor<MyOptions> options) => options.Get(""));
app.Run();