namespace Application.Features.Service.DTOs;

public class CreateServiceDto
{
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public Guid TenantId { get; set; }
}