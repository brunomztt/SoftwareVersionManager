namespace SoftwareVersionManager.Models;

public class SoftwareVersion
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public required string VersionNumber { get; set; }
    public string? ReleaseNotes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsDeprecated { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Software? Software { get; set; }
}
