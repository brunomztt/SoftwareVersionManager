using SoftwareVersionManager.DTOs;
using SoftwareVersionManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareVersionManager.Controllers;

[ApiController]
[Route("api/softwares/{softwareId}/versions")]
public class SoftwareVersionsController : ControllerBase
{
    private readonly ISoftwareVersionService _versionService;
    private readonly ISoftwareService _softwareService;
    private readonly ILogger<SoftwareVersionsController> _logger;

    public SoftwareVersionsController(
        ISoftwareVersionService versionService,
        ISoftwareService softwareService,
        ILogger<SoftwareVersionsController> logger)
    {
        _versionService = versionService;
        _softwareService = softwareService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<SoftwareVersionDto>>> GetVersionsBySoftware(int softwareId)
    {
        _logger.LogInformation("Recuperando versões do software: {softwareId}", softwareId);

        var software = await _softwareService.GetSoftwareByIdAsync(softwareId);
        if (software == null)
            return NotFound(new { message = $"Software com ID {softwareId} não encontrado" });

        try
        {
            var versions = await _versionService.GetVersionsBySoftwareIdAsync(softwareId);

            var dtos = versions.Select(v => new SoftwareVersionDto
            {
                Id = v.Id,
                SoftwareId = v.SoftwareId,
                VersionNumber = v.VersionNumber,
                ReleaseNotes = v.ReleaseNotes,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar versões");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{versionId}")]
    public async Task<ActionResult<SoftwareVersionDto>> GetVersion(int softwareId, int versionId)
    {
        _logger.LogInformation("Recuperando versão {versionId} do software {softwareId}", versionId, softwareId);

        var version = await _versionService.GetVersionByIdAsync(versionId);
        if (version == null || version.SoftwareId != softwareId)
            return NotFound(new { message = "Versão não encontrada" });

        var dto = new SoftwareVersionDto
        {
            Id = version.Id,
            SoftwareId = version.SoftwareId,
            VersionNumber = version.VersionNumber,
            ReleaseNotes = version.ReleaseNotes,
            ReleaseDate = version.ReleaseDate,
            IsDeprecated = version.IsDeprecated,
            CreatedAt = version.CreatedAt,
            UpdatedAt = version.UpdatedAt
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<SoftwareVersionDto>> CreateVersion(int softwareId, [FromBody] CreateSoftwareVersionDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Criando nova versão para software: {softwareId}", softwareId);
            var version = await _versionService.CreateVersionAsync(
                softwareId,
                createDto.VersionNumber,
                createDto.ReleaseNotes,
                createDto.ReleaseDate,
                createDto.IsDeprecated
            );

            var dto = new SoftwareVersionDto
            {
                Id = version.Id,
                SoftwareId = version.SoftwareId,
                VersionNumber = version.VersionNumber,
                ReleaseNotes = version.ReleaseNotes,
                ReleaseDate = version.ReleaseDate,
                IsDeprecated = version.IsDeprecated,
                CreatedAt = version.CreatedAt,
                UpdatedAt = version.UpdatedAt
            };

            return CreatedAtAction(nameof(GetVersion), new { softwareId, versionId = version.Id }, dto);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Software não encontrado");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar versão");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{versionId}")]
    public async Task<ActionResult<SoftwareVersionDto>> UpdateVersion(int softwareId, int versionId, [FromBody] UpdateSoftwareVersionDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var version = await _versionService.GetVersionByIdAsync(versionId);
            if (version == null || version.SoftwareId != softwareId)
                return NotFound(new { message = "Versão não encontrada" });

            _logger.LogInformation("Atualizando versão: {versionId}", versionId);
            var updatedVersion = await _versionService.UpdateVersionAsync(
                versionId,
                updateDto.VersionNumber,
                updateDto.ReleaseNotes,
                updateDto.ReleaseDate,
                updateDto.IsDeprecated
            );

            var dto = new SoftwareVersionDto
            {
                Id = updatedVersion.Id,
                SoftwareId = updatedVersion.SoftwareId,
                VersionNumber = updatedVersion.VersionNumber,
                ReleaseNotes = updatedVersion.ReleaseNotes,
                ReleaseDate = updatedVersion.ReleaseDate,
                IsDeprecated = updatedVersion.IsDeprecated,
                CreatedAt = updatedVersion.CreatedAt,
                UpdatedAt = updatedVersion.UpdatedAt
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar versão");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{versionId}")]
    public async Task<IActionResult> DeleteVersion(int softwareId, int versionId)
    {
        _logger.LogInformation("Deletando versão {versionId} do software {softwareId}", versionId, softwareId);

        var version = await _versionService.GetVersionByIdAsync(versionId);
        if (version == null || version.SoftwareId != softwareId)
            return NotFound(new { message = "Versão não encontrada" });

        var result = await _versionService.DeleteVersionAsync(versionId);
        if (!result)
            return NotFound(new { message = "Erro ao deletar versão" });

        return NoContent();
    }
}
