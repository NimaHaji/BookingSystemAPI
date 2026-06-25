namespace Application.Features.Payment.DTOs;

public class ZarinPalVerifyResponse
{
    public ZarinPalVerifyData Data { get; set; }
}

public class ZarinPalVerifyData
{
    public int Code { get; set; }
    public string CardPan { get; set; }
    public int RefId { get; set; }
    public string CardHash { get; set; }
}