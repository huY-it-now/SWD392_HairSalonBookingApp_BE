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
                if (cb.ComboServiceComboDetails.Any(cd => cd.ComboDetail.IsDeleted))
                {
                    cb.IsDeleted = true;
                }

                if (cb.IsDeleted)
                {
                    continue;
                }

                var cbDTO = new ComboServiceDTO
                {
                    Id = cb.Id,
                    ComboServiceName = cb.ComboServiceName,
                    Price = cb.Price,
                    Image = cb.ImageUrl,
                    ComboDetails = cb.ComboServiceComboDetails
                        .Where(cd => cd.ComboDetail.IsDeleted == false)
                        .Select(cd => new ComboDetailDTO
                        {
                            Id = cd.ComboDetail.Id,
                            Content = cd.ComboDetail.Content
                        }).ToList()
                };

                cbsList.Add(cbDTO);
            }

            return new Result<object>
            {
                Error = 0,
                Message = "Print all combo services with details",
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

            var comboDetailDTOs = comboDetails.Select(detail => new
            {
                ComboDetailId = detail.ComboDetailId,
                Content = detail.ComboDetail?.Content ?? "No Content Available"
            }).ToList();

            var comboServiceDTO = new
            {
                ComboServiceId = comboService.Id,
                ComboServiceName = comboService.ComboServiceName,
                Image = comboService.ImageUrl,
                ComboDetails = comboDetailDTOs
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service details retrieved successfully",
                Data = new List<object> { comboServiceDTO }
            };
        }

        public async Task<Result<object>> AddComboService(AddComboServiceRequest createRequest)
        {
            ComboServiceValidation.Validate(_mapper.Map<ComboServiceDTO>(createRequest));

            foreach (var comboDetailId in createRequest.ComboDetailIds)
            {
                var comboDetail = await _unitOfWork.ComboDetailRepository.GetByIdAsync(comboDetailId);
                if (comboDetail == null)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "One or more specified combo details do not exist.",
                        Data = null
                    };
                }

                if (comboDetail.IsDeleted == true)
                {
                    comboDetail.IsDeleted = false;
                }
            }

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
                Price = createRequest.Price,
                ImageId = cloudinaryResult.PublicImageId,
                ImageUrl = cloudinaryResult.ImageUrl,
                IsDeleted = false,
                ComboServiceComboDetails = createRequest.ComboDetailIds.Select(id => new ComboServiceComboDetail
                {
                    ComboDetailId = id
                }).ToList()
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
            var cbs = await _unitOfWork.ComboServiceRepository.GetComboServiceById(id);

            if (cbs == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found"
                };
            }

            cbs.IsDeleted = true;

            _unitOfWork.ComboServiceRepository.Update(cbs);

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "delete successfully"
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
