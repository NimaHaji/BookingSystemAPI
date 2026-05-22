using Application.Features.Tenant.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories;

public class TenantRepository:TenantRepositoryContract
{
    private readonly AppDbContext _context;
    private readonly TenantProviderContract _tenantProvider;
    public TenantRepository(AppDbContext context, TenantProviderContract tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    public async Task RegisterTenant(Tenant tenant)
    {
        await _context
            .AddAsync(tenant);
    }

    public async Task<Tenant> GetCurrentTenantAsync()
    {
        var tenantId=_tenantProvider.GetTenantId();
        if (tenantId==Guid.Empty)
            throw new UnauthorizedAccessException("TenantId not set.");
        var tenant =_context
            .Tenants
            .FirstOrDefault(t => t.Id == tenantId);
        if (tenant == null)
            throw new NotFoundException("Tenant not found.");
        return tenant;
    }

    public async Task<bool> IsSubscriptionValidAsync()
    {
        var tenant = await GetCurrentTenantAsync();
        return tenant.IsSubscriptionValid();
    }

    public async Task SaveAsync()
    {
        await _context
            .SaveChangesAsync();
    }
}