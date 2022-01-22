using AuthService.Application.Commands.Emails;
using AuthService.Application.Commands.Registration;
using AuthService.Application.Queries.Accounts;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Accounts;

public class AccountController : AbstractMediatrController
{
    [HttpPost(AccountsRoutes.RegisterAccount)]
    public async Task<IActionResult> RegisterAccount([FromBody] RegistrationBodyDto body)
    {
        var command = new RegisterAccount.Command(
            body.Email,
            body.Password,
            body.Username);
        return await Command(command)
            .MappingContent(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpGet(AccountsRoutes.GetAccount)]
    public async Task<IActionResult> GetUser([FromRoute] Guid accountId)
    {
        var query = new GetAccount.Query(accountId);
        return await Query(query)
            .MappingContent(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpGet(AccountsRoutes.GetAccounts)]
    public async Task<IActionResult> GetAccounts([FromQuery] string pattern, [FromQuery] PaginationDto pagination)
    {
        var query = new GetAccounts.Query(pattern, Mapper.Map<Pagination>(pagination));
        return await Query(query)
            .Paging(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpDelete(AccountsRoutes.DeleteAccount)]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid accountId)
    {
        var command = new UnregisterAccount.Command(accountId);
        return await Command(command)
            .MappingContent(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpPut(AccountsRoutes.ModifyAccount)]
    public async Task<IActionResult> ModifyAccount([FromRoute] Guid userId, [FromBody] ModifyAccountBodyDto body)
    {
        var command = new ChangeEmail.Command(userId, body.Email);
        return await Command(command)
            .ReturnOk();
    }
}
