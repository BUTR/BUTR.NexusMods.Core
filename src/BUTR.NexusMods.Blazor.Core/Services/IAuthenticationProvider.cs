namespace BUTR.NexusMods.Blazor.Core.Services
{
    public interface IAuthenticationProvider
    {
        Task<string?> AuthenticateAsync(string apiKey, string? type = null, CancellationToken ct = default);
        Task<bool> ValidateAsync(CancellationToken ct = default);
        Task Deauthenticate(CancellationToken ct = default);
    }
}