

using ULearn.Domain.Shared;
using ULearn.Domain.ValueObjects;

namespace ULearn.Domain.Interfaces.Services;

public interface IStorageService
{
    Task<Result<StorageFile>> UploadFileAsync(Stream fileStream,string fileName,long fileSize);
}