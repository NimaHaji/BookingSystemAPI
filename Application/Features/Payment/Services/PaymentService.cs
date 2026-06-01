using System.Security.Cryptography;
using Application.Features.Payment.Interfaces;

namespace Application.Features.Payment.Services;

public class PaymentService:PaymentServiceContract
{
    public async Task<string> GenerateResNum()
    {
        return $"{Guid.NewGuid():N}";
    }
}