namespace Application.Features.Payment.Interfaces;

public interface PaymentServiceContract
{
    Task<string> GenerateResNum();
}