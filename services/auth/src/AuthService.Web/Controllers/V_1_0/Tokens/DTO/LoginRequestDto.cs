namespace AuthService.Web.Controllers.V_1_0.Tokens.DTO;

public record LoginRequestDto(string Email, string Username, string Password);
