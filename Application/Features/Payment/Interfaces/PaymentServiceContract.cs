using Application.Features.Payment.DTOs;

namespace Application.Features.Payment.Interfaces;

public interface PaymentServiceContract
{
    Task<string> GenerateResNum();
    Task<string> CreatePaymentAsync(CreatePaymentDto dto);
    Task<bool> ProccessCallBack(SandBoxCallBackDto dto);
    Task<bool> VerifyTransaction(string RefNum);
}