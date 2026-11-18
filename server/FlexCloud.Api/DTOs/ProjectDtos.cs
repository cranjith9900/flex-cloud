using FlexCloud.Api.Models;

namespace FlexCloud.Api.DTOs;

public record ProjectDto(
    int Id, string Name, string ClientName, ProjectStatus Status, decimal Budget,
    DateTime StartDate, DateTime? EndDate, int ContactId, string? ContactName, int DocumentCount);

public record ProjectUpsertDto(
    string Name, string ClientName, ProjectStatus Status, decimal Budget,
    DateTime StartDate, DateTime? EndDate, int ContactId);
