using BUTR.NexusMods.Core.Models;

namespace BUTR.NexusMods.Core.Services
{
    public interface IProfileProvider
    {
        Task<ProfileModel?> GetProfileAsync(CancellationToken ct = default);
    }
}