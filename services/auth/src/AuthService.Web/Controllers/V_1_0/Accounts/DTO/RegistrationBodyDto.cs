namespace AuthService.Web.Controllers.V_1_0.Accounts.DTO;

public record RegistrationBodyDto(
    string Email,
    string Password,
    string Username);
