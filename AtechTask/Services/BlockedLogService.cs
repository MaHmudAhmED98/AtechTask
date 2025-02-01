using AtechTask.IServices;
using AtechTask.Model;
using System.Collections.Concurrent;

namespace AtechTask.Services
{
    public class BlockedLogService : IBlockedLogService
    {
        private static ConcurrentBag<BlockedLog> _blockedAttemptLogs = new ConcurrentBag<BlockedLog>();

        public Task LogBlockedAttemptAsync(BlockedLog log)
        {
            _blockedAttemptLogs.Add(log);
            return Task.CompletedTask;
        }

        public Task<ApiResponse<List<BlockedLog>>> GetBlockedAttemptsAsync(int page, int pageSize)
        {
            var logs = _blockedAttemptLogs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult(new ApiResponse<List<BlockedLog>> { Success = true, Data = logs });
        }

        public async Task LogBlockedAttemptAsync(string ipAddress, string countryCode, bool isBlocked , string userAgent)
        {
            var logEntry = new BlockedLog
            {
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow,
                CountryCode = countryCode,
                BlockedStatus = isBlocked,
                UserAgent = userAgent
            };

            await LogBlockedAttemptAsync(logEntry);
        }
    }
}
