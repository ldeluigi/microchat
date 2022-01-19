namespace AuthService.Web.Controllers.V_1_0.Passwords.DTO;

public record ChangePasswordBodyDto(string OldPassword, string NewPassword);
