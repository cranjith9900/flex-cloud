using FlexCloud.Api.Models;

namespace FlexCloud.Api.DTOs;

public record DocumentDto(
    int Id, string FileName, DocumentType Type, string Notes,
    DateTime UploadedDate, int ProjectId, string? ProjectName);

public record DocumentUpsertDto(string FileName, DocumentType Type, string Notes, int ProjectId);
