using FlexCloud.Api.Models;

namespace FlexCloud.Api.DTOs;

public record LeadDto(
    int Id, string Name, string Source, LeadStatus Status, decimal EstimatedValue,
    DateTime CreatedDate, int ContactId, string? ContactName);

public record LeadUpsertDto(string Name, string Source, LeadStatus Status, decimal EstimatedValue, int ContactId);
