namespace MAS.Core.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Key { get; set; } = null!;
        public string ExpiryInMins { get; set; } = null!;
    }
}
