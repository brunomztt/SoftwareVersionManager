namespace SoftwareVersionManager.Entities;

public class SoftwareVersion
{
	public int Id { get; set; }

	public string VersionNumber { get; set; }

	public DateTime ReleaseDate { get; set; }

	public bool Deprecated { get; set; }

	public int SoftwareId { get; set; }

	public Software Software { get; set; }
}