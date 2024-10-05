using AutoMapper;
using Application.Commons;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.User;
using Domain.Entities;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<RegisterUserRequest, RegisterUserDTO>();
            CreateMap<User, UserDTO>();
        }
    }
}
