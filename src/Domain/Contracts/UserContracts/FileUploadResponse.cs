namespace Domain.Contracts.UserContracts;

public record FileUploadResponse(
    string Url,
    string PublicId
);
