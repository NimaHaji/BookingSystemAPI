using Application.Features.Tenant.DTO_s;

namespace Application.Features.Tenant.Services;

public interface TenantServiceContract
{
    Task<Guid> Register(RegisterTenantDTO dto);
}