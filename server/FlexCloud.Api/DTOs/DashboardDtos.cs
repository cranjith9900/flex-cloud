namespace FlexCloud.Api.DTOs;

public record DashboardKpisDto(
    int TotalLeads, int TotalProjects, int TotalDocuments,
    decimal PipelineValue, decimal ActiveProjectBudget, decimal ConversionRate);

public record LeadsByStatusDto(string Status, int Count, decimal TotalValue);

public record ProjectsByStatusDto(string Status, int Count, decimal TotalBudget);

public record MonthlyPipelineDto(string Period, decimal NewLeadValue, decimal ConvertedValue);
