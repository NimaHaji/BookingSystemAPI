namespace Application.Features.Payment.DTOs;

public class CreatePaymentDto
{
    public long Amount { get; set; }
    public string PhoneNumber { get; set; }
}