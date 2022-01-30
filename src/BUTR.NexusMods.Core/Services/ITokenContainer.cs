namespace BUTR.NexusMods.Core.Services
{
    public interface ITokenContainer
    {
        Task<string?> GetTokenAsync(CancellationToken ct = default);
        Task SetTokenAsync(string? token, CancellationToken ct = default);
    }
}