using Application.Features.Payment.DTOs;

namespace Application.Features.Payment.Interfaces;

public interface PaymentRepositoryContract
{
    Task CreatePaymentAsync(Domain.Entities.Payment payment);
    Task SaveAsync();
    Task<Domain.Entities.Payment?> GetPaymentByResNumAsync(string resNum);
    Task<bool> IsExistByRefNum(string refNum);
}