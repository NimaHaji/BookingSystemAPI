namespace Application.Features.Payment.DTOs;

public class ZarinPalResponse
{
    public ZarinPalData? Data { get; set; }
    public object? Error { get; set; }
}

public class ZarinPalData
{
    public string Authority { get; set; }
}