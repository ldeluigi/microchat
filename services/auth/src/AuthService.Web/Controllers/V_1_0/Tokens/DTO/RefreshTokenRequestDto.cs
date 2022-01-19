namespace AuthService.Web.Controllers.V_1_0.Tokens.DTO;

public record RefreshTokenRequestDto(string RefreshToken, string AccessToken);
