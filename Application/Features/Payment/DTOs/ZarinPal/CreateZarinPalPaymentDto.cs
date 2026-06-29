using System.Text.Json.Serialization;

namespace Application.Features.Payment.DTOs.ZarinPal;

public class CreateZarinPalPaymentDto
{   
    [JsonPropertyName("amount")]
    public long Amount { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("referrer_id")]
    public string? ReferrerId { get; set; }
    
    [JsonPropertyName("metadata")]
    public ZarinPalMataData ZarinPalMataData { get; set; }
}

public class ZarinPalMataData
{
    [JsonPropertyName("mobile")]        
    public string? Mobile { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    [JsonPropertyName("auto_verify")] public bool AutoVerify { get; set; }
}