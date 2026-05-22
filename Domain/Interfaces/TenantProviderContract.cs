namespace Domain.Interfaces;

public interface TenantProviderContract
{
    Guid GetTenantId();
    void SetTenantId(Guid tenantId);
}