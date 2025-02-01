using AtechTask.Model;

namespace AtechTask.IServices
{
    public interface IBlockedCountryService
    {
        Task<bool> IsCountryBlockedAsync(string countryCode);
        Task<bool> IsBlockExpiredAsync(string countryCode);
        Task<ApiResponse<bool>> BlockCountryAsync(string countryCode);
        Task<ApiResponse<bool>> UnblockCountryAsync(string countryCode);
        Task<ApiResponse<List<string>>> GetBlockedCountriesAsync(int page, int pageSize);
        Task<ApiResponse<bool>> TemporarilyBlockCountryAsync(TemporalBlockRequest request);
    }
}
