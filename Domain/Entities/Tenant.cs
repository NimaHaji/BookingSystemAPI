namespace Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DateJoined { get; set; }
    public DateTime ExpireSubscriptionDate { get; set; }
    public string Slug { get; set; } = null!;
    public SubsciptionStatus SubscriptionStatus { get; set; }
    public List<Appointment> Appointments { get; set; }
    public List<Service> Services { get; set; }
    public List<User> Users { get; set; }

    public bool IsSubscriptionValid()
    {
        return SubscriptionStatus == SubsciptionStatus.Active && ExpireSubscriptionDate > DateTime.UtcNow;
    }

    public void ActiveSubscription() => SubscriptionStatus = SubsciptionStatus.Active;
    public void DeActiveSubscription() => SubscriptionStatus = SubsciptionStatus.Inactive;
    private Tenant(){}
    public Tenant(string name,string slug,DateTime expireSubscriptionDate)
    {
        Id = Guid.NewGuid();
        DateJoined = DateTime.UtcNow;
        Name = name;
        Slug = slug;
        SubscriptionStatus = SubsciptionStatus.Active;
        ExpireSubscriptionDate = expireSubscriptionDate;
    }
}

public enum SubsciptionStatus
{
    Active,
    Inactive,
}