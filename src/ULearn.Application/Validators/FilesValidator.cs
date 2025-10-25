

using FluentValidation;
using Microsoft.AspNetCore.Http;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;

public class UploadFileRequestDtoValidator : AbstractValidator<UploadFileRequestDto>
{
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB
    private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".png" };

    public UploadFileRequestDtoValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
            .Must(file => file.Length <= MaxFileSize).WithMessage("File size cannot exceed 10MB.")
            .Must(file => IsValidFileExtension(file.FileName)).WithMessage("Only .pdf, .jpg, and .png files are allowed.");
    }

    private bool IsValidFileExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }
}


public class UploadFilesRequestDtoValidator : AbstractValidator<UploadFilesRequestDto>
{
    public UploadFilesRequestDtoValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("At least one file is required.");

        RuleForEach(x => x.Files)
            .SetValidator(new IFormFileValidator());
    }
}



public class IFormFileValidator : AbstractValidator<IFormFile>
{
    private const long MaxFileSize = 10 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".png" };

    public IFormFileValidator()
    {
        RuleFor(file => file)
            .NotNull().WithMessage("File is required.");

        RuleFor(file => file.Length)
            .GreaterThan(0).WithMessage("File cannot be empty.")
            .LessThanOrEqualTo(MaxFileSize).WithMessage("File size cannot exceed 10MB.");

        RuleFor(file => file.FileName)
            .Must(IsValidExtension).WithMessage("Only .pdf, .jpg, and .png files are allowed.");
    }

    private bool IsValidExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return AllowedExtensions.Contains(ext);
    }
}
