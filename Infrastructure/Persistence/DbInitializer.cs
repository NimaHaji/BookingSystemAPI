using Application.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class DbInitializer
{
    
    public static void Seed(AppDbContext context, IPasswordHasher passwordHasher,IConfiguration configuration)
    {
        if (!context.Users.Any(u => u.Role == UserRole.SuperAdmin))
        {
            var FullName=configuration["SeedSuperAdmin:FullName"];
            var Email=configuration["SeedSuperAdmin:Email"];
            var PhoneNumber=configuration["SeedSuperAdmin:PhoneNumber"];
            var Password=passwordHasher.Hash(configuration["SeedSuperAdmin:Password"]);
            var role=UserRole.SuperAdmin;
            
            var User=new User(FullName,Email,PhoneNumber,role,Password);
            
            context.Users.Add(User);
            context.SaveChanges();
        }
    }
}