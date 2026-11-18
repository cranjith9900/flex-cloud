namespace FlexCloud.Api.DTOs;

public record ContactDto(int Id, string FullName, string Email, string Phone, string Company, DateTime CreatedDate);

public record ContactUpsertDto(string FullName, string Email, string Phone, string Company);
