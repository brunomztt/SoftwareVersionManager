using SoftwareVersionManager.Data;
using SoftwareVersionManager.Models;
using Microsoft.EntityFrameworkCore;

namespace SoftwareVersionManager.Services;

public interface ISoftwareService
{
    Task<List<Software>> GetAllSoftwaresAsync();
    Task<Software?> GetSoftwareByIdAsync(int id);
    Task<Software> CreateSoftwareAsync(string name, string? description, string? developer);
    Task<Software> UpdateSoftwareAsync(int id, string name, string? description, string? developer);
    Task<bool> DeleteSoftwareAsync(int id);
}

public class SoftwareService : ISoftwareService
{
    private readonly ApplicationDbContext _context;

    public SoftwareService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Software>> GetAllSoftwaresAsync()
    {
        return await _context.Softwares
            .Include(s => s.Versions)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Software?> GetSoftwareByIdAsync(int id)
    {
        return await _context.Softwares
            .Include(s => s.Versions)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Software> CreateSoftwareAsync(string name, string? description, string? developer)
    {
        var software = new Software
        {
            Name = name,
            Description = description,
            Developer = developer
        };

        _context.Softwares.Add(software);
        await _context.SaveChangesAsync();
        return software;
    }

    public async Task<Software> UpdateSoftwareAsync(int id, string name, string? description, string? developer)
    {
        var software = await GetSoftwareByIdAsync(id)
            ?? throw new KeyNotFoundException($"Software com ID {id} não encontrado.");

        software.Name = name;
        software.Description = description;
        software.Developer = developer;
        software.UpdatedAt = DateTime.UtcNow;

        _context.Softwares.Update(software);
        await _context.SaveChangesAsync();
        return software;
    }

    public async Task<bool> DeleteSoftwareAsync(int id)
    {
        var software = await GetSoftwareByIdAsync(id);
        if (software == null)
            return false;

        _context.Softwares.Remove(software);
        await _context.SaveChangesAsync();
        return true;
    }
}
