using System.Net.Http.Json;
using System.Security.Cryptography;
using Application.Features.Payment.DTOs;
using Application.Features.Payment.DTOs.ZarinPal;
using Application.Features.Payment.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Payment.Services;

public class PaymentService : PaymentServiceContract
{
    private readonly PaymentRepositoryContract _paymentRepository;
    private readonly PaymentGatewayResolverContract _gatewayResolver;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PaymentService(PaymentRepositoryContract paymentRepository, IConfiguration configuration,
        HttpClient httpClient, PaymentGatewayResolverContract gatewayResolver)
    {
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        _httpClient = httpClient;
        _gatewayResolver = gatewayResolver;
    }

    // public async Task<string?> CreatePaymentAsync(CreatePaymentDto dto)
    // {
    //     var payment = new Domain.Entities.Payment(dto.TenantId, dto.AppointmentId, dto.Amount, dto.Gateway);
    //
    //     var provider = _gatewayResolver.Resolve(dto.Gateway);
    //     
    //     var requestResult=await provider.RequestPaymentAsync(payment,dto);
    //
    //     if (!requestResult.IsSuccess)
    //     {
    //         payment.MarkAsFailed();
    //         await _paymentRepository.SaveAsync();
    //         
    //         throw new InvalidOperationException(requestResult.ErrorMessage);
    //     }
    //     await _paymentRepository.CreatePaymentAsync(payment);
    //     await _paymentRepository.SaveAsync();
    //     return requestResult.PaymentUrl;
    public async Task<string?> CreatePaymentAsync(CreatePaymentDto dto)
    {
        var payment = new Domain.Entities.Payment(dto.TenantId, dto.AppointmentId, dto.Amount, dto.Gateway);

        var provider = _gatewayResolver.Resolve(dto.Gateway);

        var requestResult = await provider.RequestPaymentAsync(payment, dto);

        if (!requestResult.IsSuccess)
        {
            payment.MarkAsFailed();
            await _paymentRepository.SaveAsync();

            throw new InvalidOperationException(requestResult.ErrorMessage);
        }
        payment.Authority=requestResult.GatewayToken;
        await _paymentRepository.CreatePaymentAsync(payment);
        await _paymentRepository.SaveAsync();
        return requestResult.PaymentUrl;
    }

    public async Task<string?> HandleCallBackAsync(PaymentGateway gateway, SandBoxCallBackDto dto)
    {
        var provider = _gatewayResolver.Resolve(gateway);
        var result= await provider.HandleCallBackAsync(gateway, dto);
        
        return result;
    }

    public async Task<bool> VerifyTransaction(string RefNum)
    {
        var request = new
        {
            refNum = RefNum,
            TerminalNumber = _configuration["Payment:TerminalId"]
        };
        var verifyUrl = _configuration["Payment:VerifyTransactionUrl"];
        var response = await _httpClient.PostAsJsonAsync(verifyUrl, request);

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
        return result is not null;
    }

}