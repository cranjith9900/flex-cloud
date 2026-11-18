namespace FlexCloud.Api.Models;

public enum LeadStatus
{
    New,
    Qualified,
    Converted,
    Lost
}

public class Lead
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public decimal EstimatedValue { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int ContactId { get; set; }
    public Contact? Contact { get; set; }
}
