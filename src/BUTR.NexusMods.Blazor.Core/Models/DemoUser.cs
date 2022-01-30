using BUTR.NexusMods.Shared.Models;

namespace BUTR.NexusMods.Blazor.Core.Models
{
    internal readonly struct DemoUserState
    {
        public static Task<DemoUserState> CreateAsync(IHttpClientFactory factory) => Task.FromResult<DemoUserState>(
            new(new(31179975, "Pickysaurus", "demo@demo.com", "https://forums.nexusmods.com/uploads/profile/photo-31179975.png", true, true)));

        public readonly ProfileModel Profile;

        private DemoUserState(ProfileModel profile)
        {
            Profile = profile;
        }
    }

    public class DemoUser
    {


        public static async Task<DemoUser> CreateAsync(IHttpClientFactory factory) => new(await DemoUserState.CreateAsync(factory));


        public virtual ProfileModel Profile => _state.Profile;

        private readonly DemoUserState _state;

        private DemoUser(DemoUserState state) => _state = state;
        protected DemoUser() { }
    }
}