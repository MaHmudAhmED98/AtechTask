using AtechTask.IServices;
using AtechTask.Model;
using System.Collections.Concurrent;

namespace AtechTask.Services
{
    public class BlockedCountryService : IBlockedCountryService
    {
        private static ConcurrentDictionary<string, DateTime> _blockedCountries = new ConcurrentDictionary<string, DateTime>();

        public Task<bool> IsBlockExpiredAsync(string countryCode)
        {
            if (_blockedCountries.TryGetValue(countryCode, out var expirationDate))
            {
                return Task.FromResult(DateTime.UtcNow > expirationDate);
            }
            return Task.FromResult(false);
        }
        public async Task<bool> IsCountryBlockedAsync(string countryCode)
        {
            {
                var blockedCountries = await GetBlockedCountriesAsync(1, int.MaxValue);
                return blockedCountries.Data.Contains(countryCode);
            }
        }
        public Task<ApiResponse<bool>> BlockCountryAsync(string countryCode)
        {
            if (_blockedCountries.ContainsKey(countryCode))
            {
                return Task.FromResult(new ApiResponse<bool> { Success = false, Message = "Country already blocked." });
            }
            _blockedCountries.TryAdd(countryCode, DateTime.UtcNow);
            return Task.FromResult(new ApiResponse<bool> { Success = true });
        }

        public Task<ApiResponse<bool>> UnblockCountryAsync(string countryCode)
        {
            if (!_blockedCountries.ContainsKey(countryCode))
            {
                return Task.FromResult(new ApiResponse<bool> { Success = false, Message = "Country not found in blocked list." });
            }
            _blockedCountries.TryRemove(countryCode, out _);
            return Task.FromResult(new ApiResponse<bool> { Success = true });
        }

        public Task<ApiResponse<List<string>>> GetBlockedCountriesAsync(int page, int pageSize)
        {
            var blockedCountries = _blockedCountries.Keys.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult(new ApiResponse<List<string>> { Success = true, Data = blockedCountries });
        }

        public Task<ApiResponse<bool>> TemporarilyBlockCountryAsync(TemporalBlockRequest request)
        {
            if (_blockedCountries.ContainsKey(request.CountryCode))
            {
                return Task.FromResult(new ApiResponse<bool> { Success = false, Message = "Country already blocked." });
            }
            _blockedCountries.TryAdd(request.CountryCode, DateTime.UtcNow.AddMinutes(request.DurationMinutes));
            return Task.FromResult(new ApiResponse<bool> { Success = true });
        }
    }
}
