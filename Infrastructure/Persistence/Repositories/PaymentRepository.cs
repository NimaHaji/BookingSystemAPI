using Application.Features.Payment.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PaymentRepository:PaymentRepositoryContract
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePaymentAsync(Payment payment)
    {
        await _dbContext
            .Payments
            .AddAsync(payment);
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Payment?> GetPaymentByResNumAsync(string resNum)
    {
        return await _dbContext
            .Payments
            .Where(p => p.ResNum == resNum)
            .FirstOrDefaultAsync();
    }

    public Task<bool> IsExistByRefNum(string refNum)
    {
        throw new NotImplementedException();
    }
}