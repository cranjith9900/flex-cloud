using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;
using FlexCloud.Api.DTOs;
using FlexCloud.Api.Models;

namespace FlexCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly FlexCloudDbContext _db;
    public ContactsController(FlexCloudDbContext db) => _db = db;

    private static ContactDto ToDto(Contact c) => new(c.Id, c.FullName, c.Email, c.Phone, c.Company, c.CreatedDate);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetAll()
    {
        var contacts = await _db.Contacts.AsNoTracking().OrderBy(c => c.FullName).ToListAsync();
        return Ok(contacts.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactDto>> GetById(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        return contact is null ? NotFound() : Ok(ToDto(contact));
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> Create(ContactUpsertDto dto)
    {
        var contact = new Contact
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Company = dto.Company,
            CreatedDate = DateTime.UtcNow
        };

        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, ToDto(contact));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ContactUpsertDto dto)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();

        contact.FullName = dto.FullName;
        contact.Email = dto.Email;
        contact.Phone = dto.Phone;
        contact.Company = dto.Company;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
