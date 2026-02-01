

using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;


namespace ULearn.Application.Services;

public class StorageService : IStorageAppService
{
    private readonly IServiceProvider _serviceProvider;

    public StorageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<StorageResponseDto>> UploadLocalFileAsync(UploadFileRequestDto requestDto)
    {
        var storage = _serviceProvider.GetKeyedService<IStorageService>("local");
        if (storage == null)
            return Result.Failure<StorageResponseDto>(new Error(Domain.Enums.ErrorCodeEnum.BadRequest, "Local storage service not found!"));

        await using var stream = requestDto.File.OpenReadStream();

        var result = await storage.UploadFileAsync(stream, requestDto.File.FileName, requestDto.File.Length);
        if (!result.IsSuccess)
            return Result.Failure<StorageResponseDto>(result.Error);

        var obj = result.Value;
        return Result.Success(new StorageResponseDto(obj.OriginalFileName, obj.FileSize, obj.FileUri));
    }

    public async Task<Result<List<StorageResponseDto>>> UploadLocalFilesAsync(UploadFilesRequestDto requestDto)
    {
        var storage = _serviceProvider.GetKeyedService<IStorageService>("local");
        if (storage == null)
            return Result.Failure<List<StorageResponseDto>>(new Error(Domain.Enums.ErrorCodeEnum.BadRequest, "Local storage service not found!"));


        var list = new List<StorageResponseDto>();

        foreach (var file in requestDto.Files)
        {
            await using var stream = file.OpenReadStream();

            var result = await storage.UploadFileAsync(stream, file.FileName, file.Length);
            if (!result.IsSuccess)
            {
                return Result.Failure<List<StorageResponseDto>>(result.Error);
            }
            var obj = result.Value;
            list.Add(new StorageResponseDto(obj.OriginalFileName, obj.FileSize, obj.FileUri));
        }

        return Result.Success(list);
    }

}