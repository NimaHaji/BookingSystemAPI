using Application.Features.Tenant.DTO_s;

namespace Application.Features.Tenant.Interfaces;

public interface TenantRepositoryContract
{
    Task RegisterTenant(Domain.Entities.Tenant tenant);
    Task<Domain.Entities.Tenant> GetCurrentTenantAsync();
    Task<bool> IsSubscriptionValidAsync();
    Task SaveAsync();
}