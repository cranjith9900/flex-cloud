namespace FlexCloud.Api.Models;

public class Contact
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
