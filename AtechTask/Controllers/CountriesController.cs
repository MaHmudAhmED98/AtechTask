using AtechTask.IServices;
using AtechTask.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtechTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockedCountryService _blockedCountryService;

        public CountriesController(IBlockedCountryService blockedCountryService)
        {
            _blockedCountryService = blockedCountryService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string countryCode)
        {
            var result = await _blockedCountryService.BlockCountryAsync(countryCode);
            return result.Success ? Ok(result) : Conflict(result.Message);
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {

            var result = await _blockedCountryService.UnblockCountryAsync(countryCode);
            return result.Success ? Ok(result) : NotFound(result.Message);
        }

        [HttpGet("blocked")]
        public async Task<IActionResult> GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _blockedCountryService.GetBlockedCountriesAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPost("temporal-block")]
        public async Task<IActionResult> TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            var result = await _blockedCountryService.TemporarilyBlockCountryAsync(request);
            return result.Success ? Ok(result) : Conflict(result.Message);
        }
    }
}
