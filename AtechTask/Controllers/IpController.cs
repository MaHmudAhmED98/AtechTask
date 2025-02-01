using AtechTask.IServices;
using AtechTask.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AtechTask.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IlocationService _locationService;
        private readonly IBlockedCountryService _blockedCountryService;
        private readonly IBlockedLogService _blockedLogService;

        public IpController(IlocationService locationService, IBlockedCountryService blockedCountryService, IBlockedLogService blockedLogService)
        {
            _locationService = locationService;
            _blockedCountryService = blockedCountryService;
            _blockedLogService = blockedLogService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string ipAddress)
        {
            var countryCode = await _locationService.GetCountryCodeFromIpAsync(ipAddress);
            return Ok(new { CountryCode = countryCode });
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            //var ipAddress = GetUserIpAddress(HttpContext);
            var ipAddress = "156.198.231.46";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var countryCode = await _locationService.GetCountryCodeFromIpAsync(ipAddress);
            var isBlocked = await _blockedCountryService.IsCountryBlockedAsync(countryCode);
            await _blockedLogService.LogBlockedAttemptAsync(ipAddress, countryCode, isBlocked, userAgent);

            return Ok(new { IsBlocked = isBlocked,CountryCode = countryCode });
        }
        private static string GetUserIpAddress(HttpContext context)
        {
            string header = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ??
                            context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (IPAddress.TryParse(header, out IPAddress ip))
            {
                return ip.ToString();
            }

            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}
