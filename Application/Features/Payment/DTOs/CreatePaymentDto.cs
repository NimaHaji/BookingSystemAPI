using Domain.Entities;

namespace Application.Features.Payment.DTOs;

public class CreatePaymentDto
{
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public long Amount { get; set; }
}