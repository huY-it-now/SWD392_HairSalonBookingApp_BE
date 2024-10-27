using CloudinaryDotNet.Actions;
using Domain.Contracts.Abstracts.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<CloudinaryResponse> UploadImage(string fileName, IFormFile fileImage);
        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}
