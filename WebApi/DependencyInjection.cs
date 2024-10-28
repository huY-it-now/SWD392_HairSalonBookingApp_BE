using Application.Interfaces;
using Application.Services;
using FluentValidation.AspNetCore;
using System.Diagnostics;
using WebApi.Services;
using WebAPI.Services;

namespace WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddHttpContextAccessor();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddScoped<IComboDetail, ComboDetailService>();
            services.AddScoped<IComboService, ComboServiceService>();

            return services;
        }
    }
}
