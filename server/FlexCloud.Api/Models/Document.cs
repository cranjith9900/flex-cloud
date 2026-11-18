namespace FlexCloud.Api.Models;

public enum DocumentType
{
    FloorPlan,
    Contract,
    Invoice,
    RenderImage,
    Other
}

public class Document
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DocumentType Type { get; set; } = DocumentType.Other;
    public string Notes { get; set; } = string.Empty;
    public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}
