using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantConfiguration:IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        
        builder.HasKey(x=>x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(p => p.DateJoined)
            .IsRequired();

        builder
            .Property(p => p.ExpireSubscriptionDate)
            .IsRequired();
        
        builder
            .Property(p=>p.Slug)
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();
    }
}