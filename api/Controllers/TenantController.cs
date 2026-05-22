using Application.Features.Tenant.DTO_s;
using Application.Features.Tenant.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly TenantServiceContract _tenantService;
    private readonly TenantProviderContract _tenantProvider;

    public TenantController(TenantServiceContract tenantService, TenantProviderContract tenantProvider)
    {
        _tenantService = tenantService;
        _tenantProvider = tenantProvider;
    }

    [HttpPost]
    [Authorize(Roles =  "User,Admin")]
    public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantDTO dto)
    {
        // Todo: expire date base on plan 
        var result=await _tenantService.Register(dto);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTenantId()
    {
        var tenantId=_tenantProvider.GetTenantId();
        return Ok(new 
        { 
            tenantId = tenantId.ToString(),
            isAuthenticated = User.Identity?.IsAuthenticated,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    } 
}