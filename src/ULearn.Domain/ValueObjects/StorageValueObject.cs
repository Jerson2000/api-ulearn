

namespace ULearn.Domain.ValueObjects;

public record StorageFile(
    string OriginalFileName,
    string StorageFileName,
    string FileUri,
    long FileSize
);
