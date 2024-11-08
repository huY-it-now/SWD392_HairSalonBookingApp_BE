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
using Domain.Contracts.DTO.Stylist;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Feedback;

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

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<UpdateProfileDTO, User>();

            CreateMap<User, UpdateProfileDTO>();

            CreateMap<UpdateProfileRequest, UpdateProfileDTO>();

            CreateMap<CreateStylistDTO, CreateStylistRequest>();

            CreateMap<CreateStylistRequest, CreateStylistDTO>();

            CreateMap<SalonMember, StylistDTO>()
                .ForMember(dest => dest.FullName, opt => 
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => 
                    opt.MapFrom(src => src.User.Email));

            CreateMap<CreateStylistDTO, StylistDTO>();

            CreateMap<ResetPasswordRequest, ResetPasswordDTO>();

            CreateMap<AddComboServiceRequest, ComboServiceDTO>();

            CreateMap<ComboService, ComboServiceDTO>()
                .ForMember(dest => dest.Image, opt => 
                    opt.MapFrom(src => src.ImageUrl));

            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.CustomerName, opt =>
                    opt.MapFrom(src => src.SalonMember.User != null ? src.SalonMember.User.FullName : "N/A"))
                .ForMember(dest => dest.StylistName, opt =>
                    opt.MapFrom(src => src.SalonMember.Id))
                .ForMember(dest => dest.StylistName, opt =>
                    opt.MapFrom(src => src.SalonMember.User.FullName));

            CreateMap<SalonMemberDTO, SalonMember>();

            CreateMap<SalonMember, SalonMemberDTO>()
                .ForMember(dest => dest.FullName, opt => 
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => 
                    opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.SalonName, opt => 
                    opt.MapFrom(src => src.Salon.salonName));


            //cbs
            CreateMap<ComboService, ComboServiceDTO>();

            CreateMap<AddComboServiceRequest, ComboService>()
                .ForMember(dest => dest.Id, opt => 
                    opt.Ignore()); // Ignore Id for creation

            CreateMap<UpdateComboServiceRequest, ComboService>();

            CreateMap<UpdateComboServiceRequest, ComboServiceDTO>();

            //cbt
            CreateMap<ComboDetail, ComboDetailDTO>();

            CreateMap<AddComboDetailRequest, ComboDetail>()
                .ForMember(dest => dest.Id, opt => 
                    opt.Ignore());

            CreateMap<UpdateComboDetailRequest, ComboDetail>();

            CreateMap<AddComboDetailRequest, ComboDetailDTO>();

            CreateMap<ComboServiceComboDetail, ComboDetailDTO>()
                .ForMember(dest => dest.Id, opt => 
                    opt.MapFrom(src => src.ComboDetailId));

            CreateMap<UpdateComboDetailRequest, ComboDetailDTO>();

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

            CreateMap<SearchSalonWithIdRequest, SalonDTO>();

            //Schedule
            CreateMap<SalonMemberSchedule, ScheduleDTO>()
                .ForMember(dest => dest.WorkShift, opt => 
                    opt.MapFrom(src => src.WorkShifts))
                .ForMember(dest => dest.IsDayOff, opt => 
                    opt.MapFrom(src => src.IsDayOff))
                .ForMember(dest => dest.Date, opt => 
                    opt.MapFrom(src => src.ScheduleDate));

            CreateMap<RegisterWorkScheduleDTO, SalonMemberSchedule>()
                .ForMember(dest => dest.WorkShifts, opt =>
                    opt.MapFrom(src => src.WorkShifts));

            CreateMap<ViewSalonDTO, ViewSalonRequest>();

            CreateMap<ViewSalonRequest, ViewSalonDTO>();

            CreateMap<SalonMember, ViewSalonMemberDTO>()
                .ForMember(dest => dest.SalonMemberId, opt => 
                    opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => 
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => 
                    opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Job, opt => 
                    opt.MapFrom(src => src.Job));

            //Appointment
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.CustomerName, opt => 
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ServiceName, opt => 
                    opt.MapFrom(src => src.ComboService.ComboServiceName))
                .ForMember(dest => dest.Status, opt => 
                    opt.MapFrom(src => src.Status));

            CreateMap<UpdateBookingStatusDTO, Booking>()
                .ForMember(dest => dest.BookingStatus, opt => 
                    opt.MapFrom(src => src.Status));

            //Booking 
            CreateMap<Booking, BookingDTO>();

            CreateMap<Booking, ViewCheckedBookingDTO>()
                .ForMember(dest => dest.BookingId, opt => 
                    opt.MapFrom(src => src.Id));

            CreateMap<Booking, ViewPendingBookingDTO>()
                .ForMember(dest => dest.BookingId, opt => 
                    opt.MapFrom(src => src.Id));

            CreateMap<Booking, BookingStatusDTO>()
                .ForMember(dest => dest.StylistName, opt => 
                    opt.MapFrom(src => src.SalonMember.Id))
                .ForMember(dest => dest.StylistName, opt => 
                    opt.MapFrom(src => src.SalonMember.User.FullName))
                .ForMember(dest => dest.PaymentStatus, opt => 
                    opt.MapFrom(src => src.Payments.PaymentStatus.StatusName))
                .ForMember(dest => dest.Feedback, opt => 
                    opt.MapFrom(src => src.Feedback.Title));
                    
            CreateMap<BookingStatusDTO, Booking>();

            CreateMap<BookingForStylist, Booking>();

            CreateMap<Booking, BookingForStylist>();

            //Feedback
            CreateMap<Booking, ListFeedbackDTO>()
                .ForMember(dest => dest.Title, opt => 
                    opt.MapFrom(src => src.Feedback.Title))
                .ForMember(dest => dest.UserName, opt => 
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Description, opt => 
                    opt.MapFrom(src => src.Feedback.Description)); ;
        }
    }
}
