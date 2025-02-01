using AtechTask.Model;

namespace AtechTask.IServices
{
    public interface IBlockedLogService
    {
        Task LogBlockedAttemptAsync(BlockedLog log);
        Task<ApiResponse<List<BlockedLog>>> GetBlockedAttemptsAsync(int page, int pageSize);
        Task LogBlockedAttemptAsync(string ipAddress, string countryCode, bool isBlocked,string userAgent);
    }
}
