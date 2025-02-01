using AtechTask.IServices;

namespace AtechTask.BackgroundServices
{
    public class BlockedCountriesBackgroundService : BackgroundService
    {
        private readonly IBlockedCountryService _blockedCountryService;

        public BlockedCountriesBackgroundService(IBlockedCountryService blockedCountryService)
        {
            _blockedCountryService = blockedCountryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var blockedCountries = await _blockedCountryService.GetBlockedCountriesAsync(1, int.MaxValue);
                foreach (var country in blockedCountries.Data)
                {
                    if (await _blockedCountryService.IsBlockExpiredAsync(country))
                    {
                        await _blockedCountryService.UnblockCountryAsync(country);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
