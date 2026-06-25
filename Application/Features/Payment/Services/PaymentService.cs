using System.Net.Http.Json;
using System.Security.Cryptography;
using Application.Features.Payment.DTOs;
using Application.Features.Payment.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Payment.Services;

public class PaymentService : PaymentServiceContract
{
    private readonly PaymentRepositoryContract _paymentRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    public PaymentService(PaymentRepositoryContract paymentRepository, IConfiguration configuration, HttpClient httpClient)
    {
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> GenerateResNum()
    {
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
        return $"{timestamp}{shortGuid}";
    }

    public async Task<string> CreatePaymentAsync(CreatePaymentDto dto)
    {
        var resNum = await GenerateResNum();
        var payment = new Domain.Entities.Payment(dto.TenantId, dto.AppointmentId, dto.Amount, resNum);
        await _paymentRepository.CreatePaymentAsync(payment);
        await _paymentRepository.SaveAsync();
        return resNum;
    }

    public async Task<bool> ProccessCallBack(SandBoxCallBackDto dto)
    {
        // if (dto.State != "OK")
        //     throw new Exception("پرداخت ناموفق");
        if (dto.ResNum == null)
            throw new Exception("شماره رزرو نامعتبر است .");
        var payment = await _paymentRepository.GetPaymentByResNumAsync(dto.ResNum);
        if (payment == null)
            throw new Exception("تراکنش نامعتبر");
        payment.Edit(dto.Status, dto.State, dto.RRN, dto.RefNum, dto.ResNum, dto.TraceNo, dto.Amount, dto.Wage);

        if (dto.State != "OK" || dto.Status != 2)
        {
            payment.MarkAsFailed();
            await _paymentRepository.SaveAsync();
            return false;
        }
            
        var verifyTransaction= await VerifyTransaction(dto.RefNum);
        if (!verifyTransaction)
        {
            payment.MarkAsFailed();
            await _paymentRepository.SaveAsync();
            return false;
        }
        payment.MarkAsSuccess();
        await _paymentRepository.SaveAsync();
        return true;
    }

    public async Task<bool> VerifyTransaction(string RefNum)
    {
        var request = new
        {
            refNum = RefNum,
            TerminalNumber  =_configuration["Payment:TerminalId"]
        };
        var verifyUrl=_configuration["Payment:VerifyTransactionUrl"];
        var response =await _httpClient.PostAsJsonAsync(verifyUrl,request);

        if (!response.IsSuccessStatusCode)
            return false;
        
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
        return result is not null;
    }
}