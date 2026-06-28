using Domain.Entities;

namespace Application.Features.Payment.DTOs;

public class CreatePaymentDto
{
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public long Amount { get; set; }
    public PaymentGateway Gateway { get; set; }
    
    public string Description { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    
    public decimal? GatewayFee { get; set; }          
    public int? TokenExpiryInMinutes { get; set; }    
    public string? ReferrerId { get; set; }
}