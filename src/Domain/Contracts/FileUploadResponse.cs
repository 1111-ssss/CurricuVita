namespace Domain.Contracts;

public record FileUploadResponse(
    string Url,
    string PublicId
);