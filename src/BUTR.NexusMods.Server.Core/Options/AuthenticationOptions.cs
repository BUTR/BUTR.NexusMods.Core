namespace BUTR.NexusMods.Server.Core.Options
{
    public record AuthenticationOptions
    {
        public string AdminToken { get; set; } = default!;
    }
}