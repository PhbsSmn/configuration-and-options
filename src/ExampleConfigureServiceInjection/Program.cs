var builder = WebApplication.CreateBuilder(args);

#region Attempt 1
/*
builder.Services.AddOptionsWithValidateOnStart<ServiceOptions>()
    .BindConfiguration(ServiceOptions.CONFIGURATION_SECTION_PATH).Configure(options =>
    {
        switch (options.ServiceToInject)
        {
            case ServiceToInject.Alpha:
                builder.Services.AddTransient<IService, AlphaService>();
                break;
            case ServiceToInject.Beta:
                builder.Services.AddTransient<IService, BetaService>();
                break;
            case ServiceToInject.Normal:
                builder.Services.AddTransient<IService, NormalService>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    });
*/
#endregion

#region Attempt 2
/*
//builder.Services.AddTransient(_ => builder.Services);
builder.Services.AddOptionsWithValidateOnStart<ServiceOptions>()
    .BindConfiguration(ServiceOptions.CONFIGURATION_SECTION_PATH).Configure((ServiceOptions options, IServiceCollection services) =>
    {
        switch (options.ServiceToInject)
        {
            case ServiceToInject.Alpha:
                services.AddTransient<IService, AlphaService>();
                break;
            case ServiceToInject.Beta:
                services.AddTransient<IService, BetaService>();
                break;
            case ServiceToInject.Normal:
                services.AddTransient<IService, NormalService>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    });
*/
#endregion

#region Attempt 3
/*
builder.Services.AddOptionsWithValidateOnStart<ServiceOptions>().BindConfiguration(ServiceOptions.CONFIGURATION_SECTION_PATH);
using (var scope = builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var scopeProvider = scope.ServiceProvider;
    var options = scopeProvider.GetRequiredService<IOptionsMonitor<ServiceOptions>>().CurrentValue;
    switch (options.ServiceToInject)
    {
        case ServiceToInject.Alpha:
            builder.Services.AddTransient<IService, AlphaService>();
            break;
        case ServiceToInject.Beta:
            builder.Services.AddTransient<IService, BetaService>();
            break;
        case ServiceToInject.Normal:
            builder.Services.AddTransient<IService, NormalService>();
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}
*/
#endregion

#region Final
/*
builder.Services.AddOptionsWithValidateOnStart<ServiceOptions>().BindConfiguration(ServiceOptions.CONFIGURATION_SECTION_PATH);
builder.Services.AddKeyedTransient<IService, AlphaService>(nameof(ServiceToInject.Alpha));
builder.Services.AddKeyedTransient<IService, BetaService>(nameof(ServiceToInject.Beta));
builder.Services.AddKeyedTransient<IService, NormalService>(nameof(ServiceToInject.Normal));
builder.Services.AddTransient(sp =>
{
    var serviceOptions = sp.GetRequiredService<IOptionsMonitor<ServiceOptions>>().CurrentValue;
    return sp.GetRequiredKeyedService<IService>(serviceOptions.ServiceToInject.ToString());
});
*/
#endregion

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/Service", (IService service) => service.GetServiceInfo());
app.Run();
