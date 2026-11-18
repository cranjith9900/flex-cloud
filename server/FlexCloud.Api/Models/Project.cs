namespace FlexCloud.Api.Models;

public enum ProjectStatus
{
    Planning,
    InProgress,
    OnHold,
    Completed
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int ContactId { get; set; }
    public Contact? Contact { get; set; }

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
