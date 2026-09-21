using Domain.Constants;
using Domain.Contracts.UserContracts;
using Domain.ResultPattern.Result;

namespace Domain.Interfaces.Services;

public interface IFileStorageService
{
    Task<Result<FileUploadResponse>> UploadAvatar(
        Stream fileStream,
        string fileName,
        string folder = FileStorageServiceConstants.AvatarsFolder,
        CancellationToken cancellationToken = default
    );
    Task DeleteAvatar(string publicId, CancellationToken cancellationToken = default);
}