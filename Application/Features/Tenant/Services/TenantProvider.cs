using Domain.Interfaces;

namespace Application.Features.Tenant.Services;

public class TenantProvider : TenantProviderContract
{
    private Guid _tenantId;
    public Guid GetTenantId() => _tenantId;
    public void SetTenantId(Guid tenantId) => _tenantId = tenantId;
}