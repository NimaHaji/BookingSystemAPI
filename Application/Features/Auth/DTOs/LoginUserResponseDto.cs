using Domain.Entities;

namespace Application.Features.Auth.DTOs;

public record LoginUserResponseDto(
    string? AccessToken,
    string RefreshToken,
    List<IdentityResponse>? IdentityResponses);

public record IdentityResponse(
    Guid tenantId,
    string Name);
public record RefreshTokenRequest (string RefreshToken);
