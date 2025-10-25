
using Microsoft.AspNetCore.Http;
using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;

public record UploadFileRequestDto(IFormFile File) : IValidateRequest;

public record UploadFilesRequestDto(List<IFormFile> Files) : IValidateRequest;
