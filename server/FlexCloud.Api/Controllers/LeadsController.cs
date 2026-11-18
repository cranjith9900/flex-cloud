using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;
using FlexCloud.Api.DTOs;
using FlexCloud.Api.Models;

namespace FlexCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly FlexCloudDbContext _db;
    public LeadsController(FlexCloudDbContext db) => _db = db;

    private static LeadDto ToDto(Lead l) => new(
        l.Id, l.Name, l.Source, l.Status, l.EstimatedValue, l.CreatedDate, l.ContactId, l.Contact?.FullName);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeadDto>>> GetAll([FromQuery] LeadStatus? status)
    {
        var query = _db.Leads.Include(l => l.Contact).AsNoTracking().AsQueryable();
        if (status.HasValue) query = query.Where(l => l.Status == status);

        var leads = await query.OrderByDescending(l => l.CreatedDate).ToListAsync();
        return Ok(leads.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LeadDto>> GetById(int id)
    {
        var lead = await _db.Leads.Include(l => l.Contact).AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        return lead is null ? NotFound() : Ok(ToDto(lead));
    }

    [HttpPost]
    public async Task<ActionResult<LeadDto>> Create(LeadUpsertDto dto)
    {
        var lead = new Lead
        {
            Name = dto.Name,
            Source = dto.Source,
            Status = dto.Status,
            EstimatedValue = dto.EstimatedValue,
            ContactId = dto.ContactId,
            CreatedDate = DateTime.UtcNow
        };

        _db.Leads.Add(lead);
        await _db.SaveChangesAsync();
        await _db.Entry(lead).Reference(l => l.Contact).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = lead.Id }, ToDto(lead));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, LeadUpsertDto dto)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead is null) return NotFound();

        lead.Name = dto.Name;
        lead.Source = dto.Source;
        lead.Status = dto.Status;
        lead.EstimatedValue = dto.EstimatedValue;
        lead.ContactId = dto.ContactId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead is null) return NotFound();
        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
