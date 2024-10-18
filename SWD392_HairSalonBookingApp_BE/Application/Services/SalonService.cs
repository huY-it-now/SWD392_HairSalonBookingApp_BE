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
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
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
                salonName = req.SalonName,
                Address = req.Address,
                ImageId = cloudinaryResult.PublicImageId,
                ImageUrl = cloudinaryResult.ImageUrl,
            };

            await _unitOfWork.SalonRepository.AddAsync(salon);
            await _unitOfWork.SaveChangeAsync();

            var salonMapper = _mapper.Map<SalonDTO>(salon);
            salonMapper.SalonName = salon.salonName;
            salonMapper.Image = salon.ImageUrl;
            salonMapper.SalonId = salon.Id;

            return new Result<object>
            {
                Error = 0,
                Message = "Salon created successfully.",
                Data = salonMapper
            };
        }

        public async Task<Result<object>> PrintAllSalon()
        {
            var salons = await _unitOfWork.SalonRepository.GetAllSalonAsync();

            var salonDTOList = new List<SalonDTO>();

            foreach (var salon in salons)
            {
                var salonDTO = new SalonDTO
                {
                    SalonName = salon.salonName,
                    Address = salon.Address,
                    Image = salon.ImageUrl,
                    SalonId = salon.Id,
                };

                salonDTOList.Add(salonDTO);
            }

            return new Result<object>
            {
                Error = 0,
                Message = "All Salons",
                Data = salonDTOList
            };
        }

        public async Task<Result<object>> SearchSalonWithAddress(SalonDTO req) {
            var salon = await _unitOfWork.SalonRepository.GetSalonByName(req.Address);

            if (salon == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "Not found salon with this address",
                    Data = null
                };
            }

            var result = _mapper.Map<SalonDTO>(salon);
            result.Image = salon.ImageUrl;
            result.SalonId = req.SalonId;

            return new Result<object> {
                Error = 0,
                Message = "Found salon with this address",
                Data = result
            };
        }

        public async Task<Result<object>> SearchSalonById(SalonDTO req) {
            var salon = await _unitOfWork.SalonRepository.GetSalonById(req.SalonId);

            if (salon == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "Not found salon",
                    Data = null
                };
            }

            var result = _mapper.Map<SalonDTO>(req);
            result.SalonName = salon.salonName;
            result.SalonId = salon.Id;
            result.Address = salon.Address;
            result.Image = salon.ImageUrl;

            return new Result<object> {
                Error = 0,
                Message = "Salon with this id",
                Data = result
            };
        }
    }
}
