using AtechTask.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtechTask.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedLogService _blockedLogService;

        public LogsController(IBlockedLogService blockedLogService)
        {
            _blockedLogService = blockedLogService;
        }

        [HttpGet("blocked-attempts")]
        public async Task<IActionResult> GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _blockedLogService.GetBlockedAttemptsAsync(page, pageSize);
            return Ok(result);
        }
    }
}
