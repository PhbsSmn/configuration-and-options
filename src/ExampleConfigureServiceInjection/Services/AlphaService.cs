public class AlphaService : IService
{
    public AlphaService()
    {
        Console.WriteLine("AlphaService created");
    }

    public string GetServiceInfo()
    {
        return "Alpha Service";
    }
}