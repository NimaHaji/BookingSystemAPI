using System.Net.Http.Json;
using System.Text.Json;
using Application.Features.Payment.DTOs;
using Application.Features.Payment.DTOs.ZarinPal;
using Application.Features.Payment.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Implement;

public class ZarinPalPaymentGatewayProvider : PaymentGatewayProviderContract
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly PaymentRepositoryContract _repositoryContract;

    public ZarinPalPaymentGatewayProvider(IConfiguration config, HttpClient httpClient, PaymentRepositoryContract repositoryContract)
    {
        _config = config;
        _httpClient = httpClient;
        _repositoryContract = repositoryContract;
    }

    public PaymentGateway Gateway => PaymentGateway.ZarinPal;

    public async Task<PaymentGatewayRequestResult> RequestPaymentAsync(Payment payment, CreatePaymentDto dto)
    {
        payment.GenerateOrderNumber();
        var requestDto = new CreateZarinPalPaymentDto
        {
            Amount = dto.Amount,
            Description = dto.Description,
            ReferrerId = dto.ReferrerId,
            ZarinPalMataData = new ZarinPalMataData
            {
                Mobile = dto.Mobile,
                Email = dto.Email,
                OrderId = payment.ResNum
            }
        };
        var request = new
        {
            merchant_id = _config["Payment:MerchantIdZarinPal"],
            amount = requestDto.Amount,
            description = requestDto.Description,
            callback_url = _config["Payment:ZarinPalCallBackUrl"],
            matadata = requestDto.ZarinPalMataData
        };

        using var response =
            await _httpClient.PostAsJsonAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", request);

        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return PaymentGatewayRequestResult.Failed(
                ((int)response.StatusCode).ToString(),
                "خطا در ارتباط با درگاه زرین پال");
        }

        var result = JsonSerializer.Deserialize<ZarinPalResponse>(raw);
        if (result is null || string.IsNullOrWhiteSpace(result.Data.Authority))
        {
            return PaymentGatewayRequestResult.Failed(
                "INVALID_RESPONSE",
                "پاسخ نامعتبر از درگاه زرین پال");
        }

        var paymentUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + result?.Data.Authority;
        return PaymentGatewayRequestResult.Success(result.Data.Authority, paymentUrl);
    }

    public async Task<string?> HandleCallBackAsync(PaymentGateway gateway, SandBoxCallBackDto dto)
    {
        if (dto.Status != "OK")
        {
            return "پرداخت ناموفق";
        }
        var payment=await _repositoryContract.GetPaymentByAuthorityAsync(dto.Authority);
        var verifyRequest = new
        {
            merchant_id = _config["Payment:MerchantIdZarinPal"],
            amount = payment.Amount,
            authority = dto.Authority
        };
        var verifyResponse =
            await _httpClient.PostAsJsonAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", verifyRequest);

        var result = await verifyResponse.Content.ReadFromJsonAsync<ZarinPalVerifyResponse>();

        if (result.Data.Code != 100 && result.Data.Code != 101)
            return "پرداخت ناموفق .";
        payment.Edit(dto.Status, result.Data.RefId, result.Data.CardPan, result.Data.Fee);
        await _repositoryContract.SaveAsync();
        return "پرداخت با موفقیت انجام شد شماره پیگیری " + result.Data.RefId;
    }
}