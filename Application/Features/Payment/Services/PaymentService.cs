using System.Security.Cryptography;
using Application.Features.Payment.DTOs;
using Application.Features.Payment.Interfaces;

namespace Application.Features.Payment.Services;

public class PaymentService : PaymentServiceContract
{
    private readonly PaymentRepositoryContract _paymentRepository;

    public PaymentService(PaymentRepositoryContract paymentRepository)
    {
        _paymentRepository = paymentRepository;
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
        if (dto.State != "OK")
            throw new Exception("پرداخت ناموفق");
        if (dto.ResNum == null)
            throw new Exception("شماره رزرو نامعتبر است .");
        var payment = await _paymentRepository.GetPaymentByResNumAsync(dto.ResNum);
        if (payment == null)
            throw new Exception("تراکنش نامعتبر");
        payment.Edit(dto.Status, dto.State, dto.RRN, dto.RefNum, dto.ResNum, dto.TraceNo, dto.Amount, dto.Wage);
        var isValid = dto.State != "OK" || dto.Status != 2;
        await _paymentRepository.SaveAsync();

        return isValid;
    }
}