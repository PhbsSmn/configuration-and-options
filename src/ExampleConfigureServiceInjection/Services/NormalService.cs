public class NormalService : IService
{
    public NormalService()
    {
        Console.WriteLine("NormalService created");
    }

    public string GetServiceInfo()
    {
        return "Normal Service";
    }
}