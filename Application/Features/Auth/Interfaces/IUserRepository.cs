using Application.Features.Auth.DTOs;
using Domain.Entities;

namespace Application.Features.Auth.Interfaces;

public interface IUserRepository
{
    Task RegisterUserAsync(User user);
    Task<List<User>?> GetUsersByEmailAsync(string email);
    Task<List<Domain.Entities.Tenant?>> GetIdentityByEmailAsync(string email);
    Task<bool> IsUserExistsByIdAsync(Guid userId);
    Task<bool> IsUserExistsByEmailAsync(string email);
    Task SaveChangesAsync();
    Task<List<ViewUser>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAndTenantIdAsync(string email, Guid tenantId);
}