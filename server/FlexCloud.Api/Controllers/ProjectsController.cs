using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;
using FlexCloud.Api.DTOs;
using FlexCloud.Api.Models;

namespace FlexCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly FlexCloudDbContext _db;
    public ProjectsController(FlexCloudDbContext db) => _db = db;

    private static ProjectDto ToDto(Project p) => new(
        p.Id, p.Name, p.ClientName, p.Status, p.Budget, p.StartDate, p.EndDate,
        p.ContactId, p.Contact?.FullName, p.Documents?.Count ?? 0);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll([FromQuery] ProjectStatus? status)
    {
        var query = _db.Projects.Include(p => p.Contact).Include(p => p.Documents).AsNoTracking().AsQueryable();
        if (status.HasValue) query = query.Where(p => p.Status == status);

        var projects = await query.OrderBy(p => p.Name).ToListAsync();
        return Ok(projects.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _db.Projects.Include(p => p.Contact).Include(p => p.Documents)
            .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return project is null ? NotFound() : Ok(ToDto(project));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(ProjectUpsertDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            ClientName = dto.ClientName,
            Status = dto.Status,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ContactId = dto.ContactId
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        await _db.Entry(project).Reference(p => p.Contact).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, ToDto(project));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProjectUpsertDto dto)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        project.Name = dto.Name;
        project.ClientName = dto.ClientName;
        project.Status = dto.Status;
        project.Budget = dto.Budget;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.ContactId = dto.ContactId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project is null) return NotFound();
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
