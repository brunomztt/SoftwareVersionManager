using SoftwareVersionManager.Data;
using SoftwareVersionManager.Models;
using Microsoft.EntityFrameworkCore;

namespace SoftwareVersionManager.Services;

public interface ISoftwareVersionService
{
    Task<List<SoftwareVersion>> GetAllVersionsAsync();
    Task<List<SoftwareVersion>> GetVersionsBySoftwareIdAsync(Guid softwareId);
    Task<SoftwareVersion?> GetVersionByIdAsync(Guid id);
    Task<SoftwareVersion> CreateVersionAsync(Guid softwareId, string versionNumber, string? releaseNotes, DateTime releaseDate, bool isDeprecated);
    Task<SoftwareVersion> UpdateVersionAsync(Guid id, string versionNumber, string? releaseNotes, DateTime releaseDate, bool isDeprecated);
    Task<bool> DeleteVersionAsync(Guid id);
}

public class SoftwareVersionService : ISoftwareVersionService
{
    private readonly ApplicationDbContext _context;

    public SoftwareVersionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SoftwareVersion>> GetAllVersionsAsync()
    {
        return await _context.SoftwareVersions
            .Include(sv => sv.Software)
            .OrderByDescending(sv => sv.ReleaseDate)
            .ToListAsync();
    }

    public async Task<List<SoftwareVersion>> GetVersionsBySoftwareIdAsync(Guid softwareId)
    {
        var softwareExists = await _context.Softwares.AnyAsync(s => s.Id == softwareId);
        if (!softwareExists)
            throw new KeyNotFoundException($"Software com ID {softwareId} não encontrado.");

        return await _context.SoftwareVersions
            .Where(sv => sv.SoftwareId == softwareId)
            .OrderByDescending(sv => sv.ReleaseDate)
            .ToListAsync();
    }

    public async Task<SoftwareVersion?> GetVersionByIdAsync(Guid id)
    {
        return await _context.SoftwareVersions
            .Include(sv => sv.Software)
            .FirstOrDefaultAsync(sv => sv.Id == id);
    }

    public async Task<SoftwareVersion> CreateVersionAsync(Guid softwareId, string versionNumber, string? releaseNotes, DateTime releaseDate, bool isDeprecated)
    {
        var software = await _context.Softwares.FindAsync(softwareId)
            ?? throw new KeyNotFoundException($"Software com ID {softwareId} não encontrado.");

        var version = new SoftwareVersion
        {
            SoftwareId = softwareId,
            VersionNumber = versionNumber,
            ReleaseNotes = releaseNotes,
            ReleaseDate = releaseDate,
            IsDeprecated = isDeprecated
        };

        _context.SoftwareVersions.Add(version);
        await _context.SaveChangesAsync();
        return version;
    }

    public async Task<SoftwareVersion> UpdateVersionAsync(Guid id, string versionNumber, string? releaseNotes, DateTime releaseDate, bool isDeprecated)
    {
        var version = await GetVersionByIdAsync(id)
            ?? throw new KeyNotFoundException($"Versão com ID {id} não encontrada.");

        version.VersionNumber = versionNumber;
        version.ReleaseNotes = releaseNotes;
        version.ReleaseDate = releaseDate;
        version.IsDeprecated = isDeprecated;
        version.UpdatedAt = DateTime.UtcNow;

        _context.SoftwareVersions.Update(version);
        await _context.SaveChangesAsync();
        return version;
    }

    public async Task<bool> DeleteVersionAsync(Guid id)
    {
        var version = await GetVersionByIdAsync(id);
        if (version == null)
            return false;

        _context.SoftwareVersions.Remove(version);
        await _context.SaveChangesAsync();
        return true;
    }
}
