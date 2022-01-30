namespace BUTR.NexusMods.Core.Models
{
    public record ProfileModel(int UserId, string Name, string Email, string ProfileUrl, bool IsPremium, bool IsSupporter)
    {
        public string? Url => UserId != -1 ? $"https://nexusmods.com/users/{UserId}" : null;
    }
}