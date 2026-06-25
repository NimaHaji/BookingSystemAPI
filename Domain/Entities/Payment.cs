using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public Appointment apppointment { get; set; }
    public string? State { get; set; }
    public long Amount { get; set; }
    public string? Wage { get; set; }
    public string ResNum { get; set; }

    public string? RefNum { get; set; }

    public string? TraceNo { get; set; }

    public string? RRN { get; set; }

    public string? CardNumber { get; set; }

    public PaymentStatus PaymentStatus{ get; set; }
    public int? PaymentGatewayStatus { get; set; }
    public string? SecurePan { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public Payment(Guid tenantId, Guid appointmentId, long amount, string resNum)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        AppointmentId = appointmentId;
        Amount = amount;
        ResNum = resNum;
        PaymentStatus = PaymentStatus.pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Edit(int? paymentGatewayStatus, string? state, string? _RRN, string? refNum, string? resNum, string? traceNo, long amount, string? wage)
    {
        PaymentGatewayStatus= paymentGatewayStatus;
        State=state;
        RRN=_RRN;
        RefNum=refNum;
        ResNum=resNum;
        TraceNo=traceNo;
        Amount = amount;
        Wage=wage;
    }

    void SetPaidAt()
    {
        PaidAt = DateTime.UtcNow;
    }
    public void MarkAsFailed()
    {
        PaymentStatus = PaymentStatus.Failed;
    }
    public void MarkAsSuccess()
    {
        PaymentStatus = PaymentStatus.Failed;
    }
}

public enum PaymentStatus
{
    pending,
    Success,
    Failed,
    Expired
}