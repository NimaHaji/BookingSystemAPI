using Application.Features.Service.DTOs;

namespace Application.Features.Service.Services;

public interface IServiceAppService
{
    Task<string> CreateServiceAsync(CreateServiceDto createServiceDto);
    Task<string> EditServiceAsync(Guid serviceId,EditService editService);
    Task<string> DeleteServiceAsync(Guid serviceId);
    Task<List<ViewServices>> ViewAllServicesAsync();
}