


using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IStorageAppService
{
    Task<Result<StorageResponseDto>> UploadLocalFileAsync(UploadFileRequestDto requestDto);
    Task<Result<List<StorageResponseDto>>> UploadLocalFilesAsync(UploadFilesRequestDto requestDto);

}