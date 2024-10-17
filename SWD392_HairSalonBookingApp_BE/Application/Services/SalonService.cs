using Application.Interfaces;
using AutoMapper;
using Domain.Contracts.Abstracts.Cloudinary;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;
using Domain.Entities;

namespace Application.Services
{
    public class SalonService : ISalonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public SalonService(IMapper mapper, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<object>> CreateSalon(CreateSalonDTO req)
        {
            if (req.Image == null || req.Image.Length == 0)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Image is required.",
                    Data = null
                };
            }

            string fileExtension = Path.GetExtension(req.Image.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}"; // Use a GUID for a unique filename
            CloudinaryResponse cloudinaryResult = await _cloudinaryService.UploadImage(newFileName, req.Image);

            if (cloudinaryResult == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Error uploading image. Please try again.",
                    Data = null
                };
            }

            var salon = new Salon
            {
                Address = req.Address,
                ImageId = cloudinaryResult.PublicImageId,
                ImageUrl = cloudinaryResult.ImageUrl
            };

            await _unitOfWork.SalonRepository.AddAsync(salon);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Salon created successfully.",
                Data = salon
            };
        }
    }
}
