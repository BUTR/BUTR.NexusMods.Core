using System.Security.Claims;
using BUTR.NexusMods.Blazor.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BUTR.NexusMods.Blazor.Core.Services
{
    public class SimpleAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly ClaimsPrincipal _authenticated = new(new ClaimsIdentity(Array.Empty<Claim>(), "NexusMods"));
        private readonly ClaimsPrincipal _administrator = new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, ApplicationRoles.Administrator) }, "Standard"));

        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IProfileProvider _profileProvider;

        public SimpleAuthenticationStateProvider(IAuthenticationProvider authenticationProvider, IProfileProvider profileProvider)
        {
            _authenticationProvider = authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider));
            _profileProvider = profileProvider ?? throw new ArgumentNullException(nameof(profileProvider));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!await _authenticationProvider.ValidateAsync())
                return new AuthenticationState(_anonymous);

            if (await _profileProvider.GetProfileAsync() is { UserId: -1 })
                return new AuthenticationState(_administrator);

            return new AuthenticationState(_authenticated);
        }
    }
}