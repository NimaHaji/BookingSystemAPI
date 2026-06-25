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
    public PaymentController(HttpClient httpClient, IConfiguration config,
        PaymentServiceContract paymentServiceContract)
    {
        _httpClient = httpClient;
        _config = config;
        _paymentServiceContract = paymentServiceContract;
    }

    [HttpPost]
    public async Task<IActionResult> RequestToken([FromBody] CreatePaymentDto dto)
    {
        var resNum = await _paymentServiceContract.CreatePaymentAsync(dto);
        var requestBody = new
        {
            action = "token",
            TerminalId = _config["Payment:TerminalId"],
            Amount = dto.Amount,
            ResNum = resNum,
            RedirectUrl = "http://salon1.localhost:8596/api/Payment/CallBack"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response =
            await _httpClient.PostAsync("https://sandbox.banktest.ir/saman/sep.shaparak.ir/OnlinePG/OnlinePG", content);

        var result = await response.Content.ReadFromJsonAsync<SepTokenResponse>();

        if (result is null || result.status != 1)
            return BadRequest(result);

        return Ok(new CreatePaymentResponse
        {
            PayUrl = $"https://sandbox.banktest.ir/saman/sep.shaparak.ir/OnlinePG/SendToken?token={result.token}"
        });
    }

    [HttpPost]
    public async Task<IActionResult> RequestTokenZarinPal([FromBody] CreateZarinPalPaymentDto dto)
    {
        var request = new
        {
            merchant_id = _config["Payment:MerchantIdZarinPal"],
            amount = dto.Amount,
            description = dto.Description,
            callback_url = _config["Payment:ZarinPalCallBackUrl"]
        };
        var response = await _httpClient.PostAsJsonAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json",request);

        var result = await response.Content.ReadFromJsonAsync<ZarinPalResponse>();
        var paymentUrl = "https://sandbox.zarinpal.com/pg/StartPay/"+result?.Data.Authority;
        
        return Ok(paymentUrl);
    }
    [HttpPost]
    public async Task<IActionResult> CallBack()
    {
        var response=Request.Form.ToDictionary(x=>x.Key, x=>x.Value);
        var dto = new SandBoxCallBackDto
        {
            MID = response.GetValueOrDefault("MID"),
            Status = int.Parse(response.GetValueOrDefault("Status")),
            State = response.GetValueOrDefault("State"),
            RRN = response.GetValueOrDefault("RRN"),
            RefNum = response.GetValueOrDefault("RefNum"),
            ResNum = response.GetValueOrDefault("ResNum"),
            TraceNo = response.GetValueOrDefault("TraceNo"),
            Amount = long.Parse(response.GetValueOrDefault("Amount")),
            Wage = response.GetValueOrDefault("Wage"),
            CID = response.GetValueOrDefault("CID"),
            SecurePan = response.GetValueOrDefault("SecurePan"),
            Token = response.GetValueOrDefault("Token"),
        };
        await _paymentServiceContract.ProccessCallBack(dto);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ZarinPalCallBack([FromQuery] ZarinPalCallBackResponse response)
    {
        if (response.Status!="OK")
        {
            return BadRequest(response);
        }

        var verifyRequest = new
        {
            merchant_id = _config["Payment:MerchantIdZarinPal"],
            //Todo : Read from db
            amount = 50000,
            authority = response.Authority
        };
        var verifyResponse = await _httpClient.PostAsJsonAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json",verifyRequest);
        
        var result = await verifyResponse.Content.ReadFromJsonAsync<ZarinPalVerifyResponse>();
        
        // http://salon1.localhost:8596/api/Payment/ZarinPalCallBack?Authority=S00000000000000000000000000000ee1owr&Status=OK
        if (result.Data.Code!=100)
            return BadRequest(result);
        
        return Ok("پرداخت با موفقیت انجام شد شماره پیگیری "+result.Data.RefId);
    }

    // [HttpPost]
    // public async Task<IActionResult> VerifyZarinPal()
    // {
    //     var request=new
    //     {
    //         merchant_id = _config["Payment:MerchantIdZarinPal"],
    //         amount = dto.Amount,
    //         description = dto.Description,
    //         callback_url = _config["Payment:ZarinPalCallBackUrl"]
    //     }
    // }
}