using BUTR.NexusMods.Shared.Models;

namespace BUTR.NexusMods.Blazor.Core.Services
{
    public interface IProfileProvider
    {
        Task<ProfileModel?> GetProfileAsync(CancellationToken ct = default);
    }
}