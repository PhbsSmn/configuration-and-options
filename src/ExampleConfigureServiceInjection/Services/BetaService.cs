public class BetaService : IService
{
    public BetaService()
    {
        Console.WriteLine("BetaService created");
    }

    public string GetServiceInfo()
    {
        return "Beta Service";
    }
}