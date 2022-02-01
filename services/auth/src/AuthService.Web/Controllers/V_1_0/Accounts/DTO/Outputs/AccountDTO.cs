using AuthService.Application.Queries.Accounts;
using EasyDesk.CleanArchitecture.Application.Mapping;
using System;

namespace AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;

public record AccountDto(
    Guid Id,
    string Email,
    string Username);

public class AccountOutputMapping : SimpleMapping<AccountOutput, AccountDto>
{
}
