using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : BaseController
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IStorageAppService _storageAppService;
        
        public FilesController(
            ILogger<FilesController> logger,
            IStorageAppService storageAppService)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageAppService = storageAppService;
        }

        [AllowAnonymous]
        [HttpPost("upload/batch")]
        public async Task<IActionResult> UploadFilesAsync(
              [FromForm] UploadFilesRequestDto batchFile)
        {

            return await _storageAppService.UploadLocalFilesAsync(batchFile).ToActionResult();
        }


        [AllowAnonymous]
        [HttpPost("upload/single")]
        public async Task<IActionResult> UploadFileAsync(
              [FromForm] UploadFileRequestDto singleFile)
        {
            return await _storageAppService.UploadLocalFileAsync(singleFile).ToActionResult();
        }




    }
}