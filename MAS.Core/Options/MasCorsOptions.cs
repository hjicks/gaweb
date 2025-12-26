namespace MAS.Core.Options;

public class MasCorsOptions
{
    public const string SectionName = "Cors";
    public string AllowedOrigins { get; set; } = string.Empty;
    public List<string> AllowedMethods { get; set; } = new List<string>();
    public List<string> AllowedHeaders { get; set; } = new List<string>();
    public bool AllowCredentials { get; set; }
    public int MaxAge { get; set; }
}
