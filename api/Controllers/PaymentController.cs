using System.Text;
using System.Text.Json;
using Application.Features.Payment.DTOs;
using Application.Features.Payment.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PaymentController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly PaymentServiceContract _paymentServiceContract;

    public PaymentController(HttpClient httpClient, IConfiguration config, PaymentServiceContract paymentServiceContract)
    {
        _httpClient = httpClient;
        _config = config;
        _paymentServiceContract = paymentServiceContract;
    }

    [HttpPost]
    public async Task<IActionResult> RequestToken([FromBody]CreatePaymentDto dto)
    {
        var resNum = await _paymentServiceContract.GenerateResNum();
        var requestBody = new
        {
            action="token",
            TerminalId=_config["Payment:TerminalId"],
            Amount=dto.Amount,
            ResNum=resNum,
            RedirectUrl=_config["Payment:RedirectUrl"],
            CellNumber=dto.PhoneNumber
        };
        
        var content=new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");
        
        var response=await _httpClient.PostAsync("https://sandbox.banktest.ir/saman/sep.shaparak.ir/OnlinePG/OnlinePG",content);
        
        var result = await response.Content.ReadFromJsonAsync<SepTokenResponse>();

        if (result is null || result.status != 1)
            return BadRequest(result);

        return Ok(new CreatePaymentResponse
        {
            PayUrl = $"https://sandbox.banktest.ir/saman/sep.shaparak.ir/OnlinePG/SendToken?token={result.token}"
        });
    }
    [HttpPost]
    public 
}