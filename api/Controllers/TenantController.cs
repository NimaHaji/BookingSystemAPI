using Application.Features.Tenant.DTO_s;
using Application.Features.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly TenantServiceContract _tenantService;

    public TenantController(TenantServiceContract tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost]
    [Authorize(Roles =  "User,Admin")]
    public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantDTO dto)
    {
        // Todo: expire date base on plan 
        var result=await _tenantService.Register(dto);
        return Ok(result);
    }
}