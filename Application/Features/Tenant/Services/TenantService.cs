using Application.Features.Tenant.DTO_s;
using Application.Features.Tenant.Interfaces;
using Domain.Entities;
namespace Application.Features.Tenant.Services;

public class TenantService:TenantServiceContract
{
    private readonly TenantRepositoryContract _tenantRepository;

    public TenantService(TenantRepositoryContract tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<string> Register(RegisterTenantDTO dto)
    {
        //Todo: slug maker
        // Todo : Date manager
        var tenant = new Domain.Entities.Tenant(dto.Name,dto.Slug,dto.SubscriptionExpireDate);
        await _tenantRepository.RegisterTenant(tenant);
        await _tenantRepository.SaveAsync();
        return $"{tenant.Name} Registered";
    }
}