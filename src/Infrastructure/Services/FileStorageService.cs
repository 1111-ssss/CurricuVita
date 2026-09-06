using Domain.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Constants;
using Domain.Contracts;
using Domain.ResultPattern.Result;
using Domain.ResultPattern.Errors;
using Microsoft.Extensions.Options;
using Domain.Options;

namespace Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;

    public FileStorageService(IOptions<CloudinaryOptions> options)
    {
        var account = new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<Result<FileUploadResponse>> UploadAvatar(
        Stream fileStream, 
        string fileName,
        string folder = FileStorageServiceConstants.AvatarsFolder,
        CancellationToken cancellationToken = default
    )
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folder,
            Transformation = new Transformation()
                .Width(FileStorageServiceConstants.AvatarWidth)
                .Height(FileStorageServiceConstants.AvatarHeight)
                .Crop(FileStorageServiceConstants.AvatarCrop)
                .Gravity(FileStorageServiceConstants.AvatarGravity)
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.Error is not null)
        {
            return Result.Failure(Errors.AvatarUploadError with { Message = result.Error.Message });
        }

        return Result<FileUploadResponse>.Success(new FileUploadResponse(
            result.SecureUrl.ToString(),
            result.PublicId
        ));
    }

    public async Task DeleteAvatar(string publicId, CancellationToken cancellationToken = default)
    {
        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}