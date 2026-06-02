using Application.Features.Auth.DTOs;
using Application.Features.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IPasswordRecoveryService _passwordRecoveryService;
    private readonly IUserService _userService;

    public AuthController(IPasswordRecoveryService passwordRecoveryService, IUserService userService)
    {
        _passwordRecoveryService = passwordRecoveryService;
        _userService = userService;
    }

    [HttpPost("Select-Tenant")]
    public async Task<IActionResult> SelectTenant(SelectTenantRequestDto dto)
    {
        var res = await _userService.SelectTenantIdAsync(dto);
        return Ok(res);
    }

    // [HttpPost("ForgetPassword")]
    // public async Task<IActionResult> ForgetPassword(ForgetPasswordRequestDto dto)
    // {
    //     await _passwordRecoveryService.ForgetPasswordAsync(dto.Email);
    //     return Ok();
    // }
    //
    // [HttpPost("ChangePassword")]
    // public async Task<IActionResult> ChangePassword(ResetPasswordRequestDto dto)
    // {
    //     await _passwordRecoveryService.ResetPasswordAsync(
    //         dto.Email,
    //         dto.Code,
    //         dto.NewPassword);
    //     return Ok();
    // }
}