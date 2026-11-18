using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;
using FlexCloud.Api.DTOs;
using FlexCloud.Api.Models;

namespace FlexCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly FlexCloudDbContext _db;
    public DocumentsController(FlexCloudDbContext db) => _db = db;

    private static DocumentDto ToDto(Document d) => new(
        d.Id, d.FileName, d.Type, d.Notes, d.UploadedDate, d.ProjectId, d.Project?.Name);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAll([FromQuery] int? projectId)
    {
        var query = _db.Documents.Include(d => d.Project).AsNoTracking().AsQueryable();
        if (projectId.HasValue) query = query.Where(d => d.ProjectId == projectId);

        var documents = await query.OrderByDescending(d => d.UploadedDate).ToListAsync();
        return Ok(documents.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DocumentDto>> GetById(int id)
    {
        var document = await _db.Documents.Include(d => d.Project).AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        return document is null ? NotFound() : Ok(ToDto(document));
    }

    [HttpPost]
    public async Task<ActionResult<DocumentDto>> Create(DocumentUpsertDto dto)
    {
        var document = new Document
        {
            FileName = dto.FileName,
            Type = dto.Type,
            Notes = dto.Notes,
            ProjectId = dto.ProjectId,
            UploadedDate = DateTime.UtcNow
        };

        _db.Documents.Add(document);
        await _db.SaveChangesAsync();
        await _db.Entry(document).Reference(d => d.Project).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = document.Id }, ToDto(document));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var document = await _db.Documents.FindAsync(id);
        if (document is null) return NotFound();
        _db.Documents.Remove(document);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
