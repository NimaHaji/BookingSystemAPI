using Application.Features.Tenant.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories;

public class TenantRepository:TenantRepositoryContract
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegisterTenant(Tenant tenant)
    {
        await _context
            .AddAsync(tenant);
    }
    public async Task SaveAsync()
    {
        await _context
            .SaveChangesAsync();
    }
}