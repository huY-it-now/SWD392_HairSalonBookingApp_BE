using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Application.Validations.Stylist;
using FluentValidation;
using Infrastructures.Mappers;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddScoped<IPasswordHash, PasswordHash>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddScoped<ISalonService, SalonService>();

            services.AddScoped<ISalonRepository, SalonRepository>();

            services.AddScoped<IPaymentStatusRepository, PaymentStatusRepository>();

            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

            services.AddScoped<IPaymentLogRepository, PaymentLogRepository>();

            services.AddScoped<IPaymentsRepository, PaymentsRepository>();

            services.AddScoped<IScheduleWorkTimeRepository, ScheduleWorkTimeRepository>();

            services.AddScoped<ISalonMemberService, SalonMemberService>();

            services.AddScoped<ISalonMemberRepository, SalonMemberRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICategoryRepository,CategoryRepository>();

            services.AddScoped<ICategoryService,CategoryService>();

            services.AddScoped<IServiceRepository, ServiceRepository>();

            services.AddScoped<IServiceService, ServiceService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<ICurrentTime, CurrentTime>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            services.AddScoped<IComboServiceComboDetailRepository, ComboServiceComboDetailRepository>();

            services.AddScoped<IServiceComboServiceRepository, ServiceComboRepository>();

            services.AddScoped<IComboServiceRepository, ComboServiceRepository>();

            services.AddScoped<IComboDetailRepository, ComboDetailRepository>();

            services.AddScoped<ComboDetailService>();

            services.AddScoped<IComboService, ComboServiceService>();

            services.AddScoped<ISalonMemberScheduleRepository, SalonMemberScheduleRepository>();

            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            // ATTENTION: if you do migration please check file README.md
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));

            // this configuration just use in-memory for fast develop
            services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("DatabaseConnection"));

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

            return services;
        }
    }
}
