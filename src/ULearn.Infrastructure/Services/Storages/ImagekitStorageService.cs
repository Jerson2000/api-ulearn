

using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Domain.ValueObjects;

namespace ULearn.Infrastructure.Services.Storages;

public class ImagekitStorageService : IStorageService
{
    public Task<Result<StorageFile>> UploadFileAsync(Stream fileStream,string fileName,long fileSize)
    {
        throw new NotImplementedException();
    }
}