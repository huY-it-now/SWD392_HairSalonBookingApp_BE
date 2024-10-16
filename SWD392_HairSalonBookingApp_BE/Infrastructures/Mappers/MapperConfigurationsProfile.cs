using AutoMapper;
using Application.Commons;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.User;
using Domain.Entities;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Salon;
using Domain.Contracts.DTO.Salon;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<RegisterUserRequest, RegisterUserDTO>();
            CreateMap<LoginUserRequest, LoginUserDTO>();
            CreateMap<VerifyTokenRequest, VerifyTokenDTO>();
            CreateMap<User, UserDTO>();

            //cbs
            CreateMap<ComboService, ComboServiceDTO>();
            CreateMap<AddComboServiceRequest, ComboService>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id for creation
            CreateMap<UpdateComboServiceRequest, ComboService>();

            //cbt
            CreateMap<ComboDetail, ComboDetailDTO>();
            CreateMap<AddComboDetailRequest, ComboDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateComboDetailRequest, ComboDetail>();

            //Salon
            CreateMap<CreateSalonRequest, CreateSalonDTO>();
        }
    }
}
