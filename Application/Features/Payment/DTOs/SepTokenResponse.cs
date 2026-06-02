namespace Application.Features.Payment.DTOs;

public class SepTokenResponse
{
    public int status { get; set; }

    public string? token { get; set; }

    public string? errorCode { get; set; }

    public string? errorDesc { get; set; }
}