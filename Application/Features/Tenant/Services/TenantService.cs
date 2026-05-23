using Application.Common.Interfaces;
using Application.Features.Auth.Interfaces;
using Application.Features.Auth.Services;
using Application.Features.Tenant.DTO_s;
using Application.Features.Tenant.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Tenant.Services;

public class TenantService:TenantServiceContract
{
    private readonly TenantRepositoryContract _tenantRepository;
    private readonly IUSerContext _userContext;
    private  readonly IUserRepository _userRepository;

    public TenantService(TenantRepositoryContract tenantRepository, IUSerContext userContext, IUserRepository userRepository)
    {
        _tenantRepository = tenantRepository;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<Guid> Register(RegisterTenantDTO dto)
    {
        //Todo: slug maker
        // Todo : Date manager
        // Todo : if for duplicate slug
        var tenant = new Domain.Entities.Tenant(dto.Name,dto.Slug,dto.SubscriptionExpireDate);
                
        var userId=_userContext.UserId;
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if(user==null)
            throw new NotFoundException("User not found");
        
        await _tenantRepository.RegisterTenant(tenant);
        user.ChangeRoleTo(UserRole.Admin);
        user.AssignTenantToUser(tenant.Id);
        await _tenantRepository.SaveAsync();

        // var tenantId = await _tenantRepository.GetCurrentTenantAsync();
        return tenant.Id;
    }
}