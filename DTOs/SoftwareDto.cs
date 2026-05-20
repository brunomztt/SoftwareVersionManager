namespace SoftwareVersionManager.DTOs;

public class CreateSoftwareDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Developer { get; set; }
}

public class UpdateSoftwareDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Developer { get; set; }
}

public class SoftwareDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Developer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SoftwareVersionDto> Versions { get; set; } = new();
}

public class SoftwareVersionDto
{
    public Guid Id { get; set; }
    public Guid SoftwareId { get; set; }
    public required string VersionNumber { get; set; }
    public string? ReleaseNotes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsDeprecated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateSoftwareVersionDto
{
    public required string VersionNumber { get; set; }
    public string? ReleaseNotes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsDeprecated { get; set; } = false;
}

public class UpdateSoftwareVersionDto
{
    public required string VersionNumber { get; set; }
    public string? ReleaseNotes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsDeprecated { get; set; }
}
