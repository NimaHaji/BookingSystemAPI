using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext:DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Appointment> Appointments =>Set<Appointment>();
    public DbSet<Service> Services =>Set<Service>();
    public  DbSet<User> Users =>Set<User>();
    public DbSet<RefreshToken> RefreshTokens =>Set<RefreshToken>();
    public DbSet<AppointmentServiceLink> AppointmentServiceLinks =>Set<AppointmentServiceLink>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }
}