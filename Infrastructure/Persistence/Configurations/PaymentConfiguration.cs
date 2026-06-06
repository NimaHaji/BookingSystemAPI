using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.TenantId).IsRequired();
    
        builder.Property(p => p.AppointmentId).IsRequired();
        
        builder
            .Property(p => p.Amount)
            .IsRequired();
        
        builder.Property(p=>p.ResNum)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.RefNum)
            .HasMaxLength(100);
        
        builder
            .Property(p => p.TraceNo);
        
        builder
            .Property(p=>p.RRN)
            .HasMaxLength(100);
        
        builder
            .Property(p=>p.PaymentStatus)
            .IsRequired();
        
        builder
            .Property(p=>p.PaymentGatewayStatus);
        
        builder
            .Property(p => p.CreatedAt)
            .IsRequired();
        
        builder
            .Property(p=>p.PaidAt)
            .IsRequired(false);
        
        builder.HasOne(p => p.apppointment)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.AppointmentId);
        
        builder
            .HasIndex(p=>p.ResNum)
            .IsUnique();

        builder
            .HasIndex(p => p.AppointmentId);
        
        builder
            .HasIndex(p => p.TenantId);
    }
}