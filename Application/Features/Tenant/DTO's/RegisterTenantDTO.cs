namespace Application.Features.Tenant.DTO_s;

public class RegisterTenantDTO
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime SubscriptionExpireDate { get; set; }
}