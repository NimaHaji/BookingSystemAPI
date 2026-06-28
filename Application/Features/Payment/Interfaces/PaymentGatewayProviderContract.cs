using Application.Features.Payment.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Payment.Interfaces;

public interface PaymentGatewayProviderContract
{
    public PaymentGateway Gateway { get;}
    
    Task<PaymentGatewayRequestResult> RequestPaymentAsync(Domain.Entities.Payment payment, CreatePaymentDto dto);
    
    Task<string?> HandleCallBackAsync(PaymentGateway gateway, SandBoxCallBackDto dto);
}