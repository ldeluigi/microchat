using System;

namespace AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;

public record AccountDto(
    Guid Id,
    string Email,
    string Username);
