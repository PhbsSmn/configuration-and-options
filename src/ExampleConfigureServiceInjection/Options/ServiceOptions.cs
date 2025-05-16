internal class ServiceOptions
{
    public const string CONFIGURATION_SECTION_PATH = "Service";

    public ServiceToInject ServiceToInject { get; set; } = ServiceToInject.Normal;
}