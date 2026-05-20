using SoftwareVersionManager.DTOs;
using SoftwareVersionManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareVersionManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoftwaresController : ControllerBase
{
    private readonly ISoftwareService _softwareService;
    private readonly ILogger<SoftwaresController> _logger;

    public SoftwaresController(ISoftwareService softwareService, ILogger<SoftwaresController> logger)
    {
        _softwareService = softwareService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<SoftwareDto>>> GetAll()
    {
        _logger.LogInformation("Recuperando todos os softwares");
        var softwares = await _softwareService.GetAllSoftwaresAsync();

        var dtos = softwares.Select(s => new SoftwareDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Developer = s.Developer,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            Versions = s.Versions.Select(v => new SoftwareVersionDto
            {
                Id = v.Id,
                SoftwareId = v.SoftwareId,
                VersionNumber = v.VersionNumber,
                ReleaseNotes = v.ReleaseNotes,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            }).ToList()
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SoftwareDto>> GetById(Guid id)
    {
        _logger.LogInformation("Recuperando software com ID: {id}", id);
        var software = await _softwareService.GetSoftwareByIdAsync(id);

        if (software == null)
            return NotFound(new { message = $"Software com ID {id} não encontrado" });

        var dto = new SoftwareDto
        {
            Id = software.Id,
            Name = software.Name,
            Description = software.Description,
            Developer = software.Developer,
            CreatedAt = software.CreatedAt,
            UpdatedAt = software.UpdatedAt,
            Versions = software.Versions.Select(v => new SoftwareVersionDto
            {
                Id = v.Id,
                SoftwareId = v.SoftwareId,
                VersionNumber = v.VersionNumber,
                ReleaseNotes = v.ReleaseNotes,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<SoftwareDto>> Create([FromBody] CreateSoftwareDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Criando novo software: {name}", createDto.Name);
            var software = await _softwareService.CreateSoftwareAsync(
                createDto.Name,
                createDto.Description,
                createDto.Developer
            );

            var dto = new SoftwareDto
            {
                Id = software.Id,
                Name = software.Name,
                Description = software.Description,
                Developer = software.Developer,
                CreatedAt = software.CreatedAt,
                UpdatedAt = software.UpdatedAt,
                Versions = new()
            };

            return CreatedAtAction(nameof(GetById), new { id = software.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar software");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SoftwareDto>> Update(Guid id, [FromBody] UpdateSoftwareDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Atualizando software com ID: {id}", id);
            var software = await _softwareService.UpdateSoftwareAsync(
                id,
                updateDto.Name,
                updateDto.Description,
                updateDto.Developer
            );

            var dto = new SoftwareDto
            {
                Id = software.Id,
                Name = software.Name,
                Description = software.Description,
                Developer = software.Developer,
                CreatedAt = software.CreatedAt,
                UpdatedAt = software.UpdatedAt,
                Versions = software.Versions.Select(v => new SoftwareVersionDto
                {
                    Id = v.Id,
                    SoftwareId = v.SoftwareId,
                    VersionNumber = v.VersionNumber,
                    ReleaseNotes = v.ReleaseNotes,
                    ReleaseDate = v.ReleaseDate,
                    IsDeprecated = v.IsDeprecated,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                }).ToList()
            };

            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Software não encontrado");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar software");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deletando software com ID: {id}", id);
        var result = await _softwareService.DeleteSoftwareAsync(id);

        if (!result)
            return NotFound(new { message = $"Software com ID {id} não encontrado" });

        return NoContent();
    }
}
