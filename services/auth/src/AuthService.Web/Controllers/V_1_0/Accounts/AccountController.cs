using System;
using System.Threading.Tasks;
using AuthService.Application.Commands.Accounts;
using AuthService.Application.Commands.Registration;
using AuthService.Application.Queries.Accounts;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.Tools.Options;
using Microsoft.AspNetCore.Mvc;

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

    [HttpDelete(AccountsRoutes.DeleteAccount)]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid accountId)
    {
        var command = new UnregisterAccount.Command(accountId);
        return await Command(command)
            .MappingContent(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpPut(AccountsRoutes.ModifyAccount)]
    public async Task<IActionResult> ModifyAccount([FromRoute] Guid accountId, [FromBody] ModifyAccountBodyDto body)
    {
        var command = new UpdateAccount.Command(accountId, body.Username.AsOption(), body.Email.AsOption());
        return await Command(command)
            .ReturnOk();
    }
}
