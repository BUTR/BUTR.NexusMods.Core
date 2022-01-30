using Blazored.LocalStorage;

namespace BUTR.NexusMods.Core.Services
{
    public class LocalStorageTokenContainer : ITokenContainer
    {
        private readonly ILocalStorageService _localStorage;

        public LocalStorageTokenContainer(ILocalStorageService localStorage)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));

        }

        public async Task<string?> GetTokenAsync(CancellationToken ct = default)
        {
            return await _localStorage.GetItemAsStringAsync("token");
        }

        public async Task<string?> GetTokenTypeAsync(CancellationToken ct = default)
        {
            return await _localStorage.GetItemAsStringAsync("token_type");
        }

        public async Task SetTokenAsync(string? token, CancellationToken ct = default)
        {
            await _localStorage.SetItemAsStringAsync("token", token, ct);
        }

        public async Task SetTokenTypeAsync(string? tokenType, CancellationToken ct = default)
        {
            await _localStorage.SetItemAsStringAsync("token_type", tokenType, ct);
        }
    }
}