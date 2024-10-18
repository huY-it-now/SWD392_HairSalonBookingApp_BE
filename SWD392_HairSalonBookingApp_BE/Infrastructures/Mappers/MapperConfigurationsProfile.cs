﻿using AutoMapper;
using Application.Commons;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.User;
using Domain.Entities;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;
using Domain.Contracts.Abstracts.Service;
using Domain.Contracts.Abstracts.Salon;
using Domain.Contracts.DTO.Salon;
using Domain.Contracts.Abstracts.Category;
using Domain.Contracts.DTO.Stylish;

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
            CreateMap<CreateStylistDTO, CreateStylistRequest>();
            CreateMap<CreateStylistRequest, CreateStylistDTO>();
            CreateMap<SalonMember, StylistDTO>()
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

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

            //cate
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryRequest, CreateCategoryDTO>();
            CreateMap<CategoryDTO, CreateCategoryDTO>();
            CreateMap<UpdateCategoryRequest, UpdateCategoryDTO>();
            CreateMap<UpdateCategoryDTO, Category>();

            //ser
            CreateMap<Service, ServiceDTO>();
            CreateMap<CreateServiceRequest, CreateServiceDTO>();
            CreateMap<UpdateServiceRequest, UpdateServiceDTO>();
            CreateMap<UpdateServiceDTO, Service>();

            //Salon
            CreateMap<CreateSalonRequest, CreateSalonDTO>();
            CreateMap<CreateSalonDTO, SalonDTO>();
            CreateMap<Salon, SalonDTO>();
            CreateMap<SalonDTO, SearchSalonRequest>();
            CreateMap<SearchSalonRequest, SalonDTO>();

            //Schedule
            CreateMap<SalonMemberSchedule, ScheduleDTO>()
            .ForMember(dest => dest.WorkShift, opt => opt.MapFrom(src => src.WorkShift))
            .ForMember(dest => dest.IsDayOff, opt => opt.MapFrom(src => src.IsDayOff))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}
