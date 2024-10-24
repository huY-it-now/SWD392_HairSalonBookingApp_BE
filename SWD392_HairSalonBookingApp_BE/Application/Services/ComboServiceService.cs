using Application.Interfaces;
using Application.Repositories;
using Application.Validations.Combo;
using AutoMapper;
using Domain.Contracts.Abstracts.Cloudinary;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;

namespace Application.Services
{
    public class ComboServiceService : IComboService
    {
        readonly IComboServiceRepository _comboServiceRepository;
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public ComboServiceService(IComboServiceRepository comboServiceRepository,
                                   IComboServiceComboDetailRepository comboServiceComboDetailRepository,
                                   IMapper mapper,
                                   IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _comboServiceRepository = comboServiceRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<object>> GetAllComboServices()
        {
            var cbs = await _comboServiceRepository.GetAllComboServiceAsync();
            var cbsList = new List<ComboServiceDTO>();

            foreach (var cb in cbs)
            {
                var cbDTO = new ComboServiceDTO
                {
                    Id = cb.Id,
                    ComboServiceName = cb.ComboServiceName,
                    Price = cb.Price,
                    SalonId = cb.SalonId,
                    Image = cb.ImageUrl,
                    //ComboDetails = cb.ComboDetails
                };

                cbsList.Add(cbDTO);
            }

            return new Result<object>
            {
                Error = 0,
                Message = "Print all combo services",
                Data = cbsList
            };
        }

        public async Task<Result<object>> GetAllComboDetailByComboServiceId(Guid id)
        {
            var comboService = await _comboServiceRepository.GetComboServiceById(id);
            if (comboService == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Combo service not found",
                    Data = null
                };
            }

            var comboDetails = await _comboServiceRepository.GetComboDetailByComboServiceId(id);

            if (comboDetails == null || !comboDetails.Any())
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "No combo details",
                    Data = null
                };
            }

            // Create a list of combo detail objects
            var comboDetailDTOs = comboDetails.Select(detail => new
            {
                ComboDetailId = detail.ComboDetailId,
                Content = detail.ComboDetail?.Content ?? "No Content Available"
            }).ToList();

            // Create a single combo service object that includes the combo details
            var comboServiceDTO = new
            {
                ComboServiceId = comboService.Id,
                ComboServiceName = comboService.ComboServiceName,
                Image = comboService.ImageUrl,
                ComboDetails = comboDetailDTOs // Nest the combo details inside
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service details retrieved successfully",
                Data = new List<object> { comboServiceDTO } // Return as a list with one item
            };
        }

        public async Task<Result<object>> AddComboService(AddComboServiceRequest createRequest)
        {
            ComboServiceValidation.Validate(_mapper.Map<ComboServiceDTO>(createRequest));

            string fileExtension = Path.GetExtension(createRequest.ImageUrl.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            CloudinaryResponse cloudinaryResult = await _cloudinaryService.UploadImage(newFileName, createRequest.ImageUrl);

            if (cloudinaryResult == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Error uploading image. Please try again.",
                    Data = null
                };
            }

            var comboService = new ComboService
            {
                ComboServiceName = createRequest.ComboServiceName,
                SalonId = createRequest.SalonId,
                ImageId = cloudinaryResult.PublicImageId,
                ImageUrl = cloudinaryResult.ImageUrl,
                ComboServiceComboDetails = new List<ComboServiceComboDetail>
                {
                    new ComboServiceComboDetail
                    {
                        ComboDetailId = createRequest.ComboDetailId
                    }
                }
            };

            await _unitOfWork.ComboServiceRepository.AddAsync(comboService);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service added successfully",
                Data = null
            };
        }

        public async Task<Result<object>> UpdateComboService(UpdateComboServiceRequest updateRequest)
        {
            ComboServiceValidation.Validate(_mapper.Map<ComboServiceDTO>(updateRequest));
            var comboService = _mapper.Map<ComboService>(updateRequest);
            await _comboServiceRepository.UpdateComboService(comboService);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service updated successfully",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteComboService(Guid id)
        {
            await _comboServiceRepository.DeleteComboService(id);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service deleted successfully",
                Data = null
            };
        }

        public async Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(comboServiceId);
            var comboDetailDTOs = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo details for the combo service",
                Data = comboDetailDTOs
            };
        }

        public async Task<Result<object>> AddComboDetailIntoComboService(Guid comboServiceId, Guid comboDetailId)
        {
            var comboService = await _unitOfWork.ComboServiceRepository.GetComboServiceById(comboServiceId);
            var comboDetail = await _unitOfWork.ComboDetailRepository.GetComboDetailById(comboDetailId);

            if (comboService == null || comboDetail == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Combo service or detail not found",
                    Data = null
                };
            }

            var comboServiceComboDetail = new ComboServiceComboDetail
            {
                ComboServiceId = comboServiceId,
                ComboDetailId = comboDetailId,
                ComboService = comboService,
                ComboDetail = comboDetail
            };

            await _unitOfWork.ComboServiceComboDetailRepository.AddAsync(comboServiceComboDetail);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Combo detail added to combo service successfully",
                Data = null
            };
        }
    }
}
