using Application.Features.Appointments.Interfaces;
using Application.Features.Appointments.Services;
using Application.Features.Auth.Interfaces;
using Application.Features.Auth.Services;
using Application.Features.Payment.Interfaces;
using Application.Features.Payment.Services;
using Application.Features.Service.Services;
using Application.Features.Tenant.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentService,AppointmentService>();
        services.AddScoped<IServiceAppService,ServiceAppService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
        services.AddScoped<TenantServiceContract,TenantService>();
        services.AddScoped<TenantProviderContract, TenantProvider>();
        services.AddScoped<PaymentServiceContract, PaymentService>();
        return services;
    }
}