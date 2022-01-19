namespace AuthService.Web.Controllers.V_1_0.Passwords.DTO;

public record AcceptPasswordRecoveryBodyDto(string Token, string NewPassword);
