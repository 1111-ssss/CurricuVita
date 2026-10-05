using System.Text.Json;
using Domain.Contracts.SupportContracts;
using Domain.Interfaces.Services;
using Domain.Options;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class FileSupportTicketSink : ISupportTicketSink
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly SupportTicketOptions _options;
    private readonly IHostEnvironment _environment;

    public FileSupportTicketSink(
        IOptions<SupportTicketOptions> options,
        IHostEnvironment environment
    )
    {
        _options = options.Value;
        _environment = environment;
    }

    public async Task<Result> Store(
        string fileName,
        SupportTicketPayload payload,
        CancellationToken cancellationToken = default
    )
    {
        var dir = Path.IsPathRooted(_options.StorageDir)
            ? _options.StorageDir
            : Path.Combine(_environment.ContentRootPath, _options.StorageDir);

        try
        {
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, fileName);
            var json = JsonSerializer.Serialize(payload, SerializerOptions);
            await File.WriteAllTextAsync(
                path,
                json,
                cancellationToken
            );
            return Result.Success();
        }
        catch
        {
            return Result.Failure(Errors.SupportTicketStoreError);
        }
    }
}
