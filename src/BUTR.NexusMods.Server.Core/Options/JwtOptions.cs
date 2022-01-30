namespace BUTR.NexusMods.Server.Core.Options
{
    public record JwtOptions
    {
        public string SignKey { get; set; } = default!;
        public string EncryptionKey { get; set; } = default!;
    }
}