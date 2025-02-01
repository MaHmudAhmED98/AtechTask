namespace AtechTask.IServices
{
    public interface IlocationService
    {
        Task<string> GetCountryCodeFromIpAsync(string ipAddress);

    }
}
