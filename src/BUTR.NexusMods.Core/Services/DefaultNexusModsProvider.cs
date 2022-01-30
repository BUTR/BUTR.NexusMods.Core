using BUTR.NexusMods.Core.Models;

using Microsoft.Extensions.Options;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BUTR.NexusMods.Core.Services
{
    public class DefaultNexusModsProvider : IAuthenticationProvider, IProfileProvider
    {
        private record JwtTokenResponse(string Token);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenContainer _tokenContainer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DefaultNexusModsProvider(IHttpClientFactory httpClientFactory, ITokenContainer tokenContainer, IOptions<JsonSerializerOptions> jsonSerializerOptions)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tokenContainer = tokenContainer ?? throw new ArgumentNullException(nameof(tokenContainer));
            _jsonSerializerOptions = jsonSerializerOptions.Value ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
        }

        public async Task<string?> AuthenticateAsync(string apiKey, string? type = null, CancellationToken ct = default)
        {
            await _tokenContainer.SetTokenTypeAsync(type, ct);

            if (type?.Equals("demo", StringComparison.OrdinalIgnoreCase) == true)
            {
                return "";
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"Authentication/Authenticate{(string.IsNullOrEmpty(type) ? string.Empty : $"?type={type}")}");
                request.Headers.Add("apikey", apiKey);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpClient = _httpClientFactory.CreateClient("Backend");
                var response = await httpClient.SendAsync(request, ct);
                if (response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<JwtTokenResponse>(_jsonSerializerOptions, ct) is { } json)
                {
                    await _tokenContainer.SetTokenAsync(json.Token, ct);
                    return json.Token;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                await _tokenContainer.SetTokenTypeAsync(null, ct);
                return null;
            }
        }

        public async Task<bool> ValidateAsync(CancellationToken ct = default)
        {
            var tokenType = await _tokenContainer.GetTokenTypeAsync(ct);
            if (tokenType?.Equals("demo", StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            var token = await _tokenContainer.GetTokenAsync(ct);
            if (token is null)
            {
                return false;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "Authentication/Validate");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpClient = _httpClientFactory.CreateClient("Backend");
                var response = await httpClient.SendAsync(request, ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task Deauthenticate(CancellationToken ct = default)
        {
            await _tokenContainer.SetTokenAsync(null, ct);
            await _tokenContainer.SetTokenTypeAsync(null, ct);
        }

        public async Task<ProfileModel?> GetProfileAsync(CancellationToken ct = default)
        {
            var tokenType = await _tokenContainer.GetTokenTypeAsync(ct);
            if (tokenType?.Equals("demo", StringComparison.OrdinalIgnoreCase) == true)
            {
                var demoUser = await DemoUser.CreateAsync(_httpClientFactory);
                return demoUser.Profile;
            }

            var token = await _tokenContainer.GetTokenAsync(ct);
            if (token is null)
                return null;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "Authentication/Profile");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpClient = _httpClientFactory.CreateClient("Backend");
                var response = await httpClient.SendAsync(request, ct);
                return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProfileModel>(_jsonSerializerOptions, ct) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}