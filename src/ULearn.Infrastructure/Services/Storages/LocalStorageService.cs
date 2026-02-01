

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Domain.ValueObjects;

namespace ULearn.Infrastructure.Services.Storages;

public class LocalStorageService : IStorageService
{
    private const string UploadsFolderName = "uploads";
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB limit
    private ILogger<LocalStorageService> _logger;
    private readonly IWebHostEnvironment _environment;
    private string? uploadsFolder;

    public LocalStorageService(ILogger<LocalStorageService> logger, IWebHostEnvironment environment)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        uploadsFolder = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "..", "..", UploadsFolderName));
        if (!File.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

    }
    public async Task<Result<StorageFile>> UploadFileAsync(Stream fileStream, string fileName, long fileSize)
    {
        try
        {
            if (string.IsNullOrEmpty(uploadsFolder)) return Result.Failure<StorageFile>(Domain.Enums.ErrorCodeEnum.InternalServerError, "Upload directory is not found or missing.");

            var storageFileName = $"{new Random().Next(1000000)}_{fileName}";
            var filePath = Path.Combine(uploadsFolder, storageFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            return Result.Success(new StorageFile(fileName, storageFileName, $"/uploads/{storageFileName}", fileSize));
        }
        catch (Exception ex)
        {
            return Result.Failure<StorageFile>(Domain.Enums.ErrorCodeEnum.InternalServerError, $"An unexpected error occured: {ex.Message}");
        }

    }
}