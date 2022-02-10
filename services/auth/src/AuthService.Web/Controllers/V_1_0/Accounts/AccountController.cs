using System;
using System.Threading.Tasks;
using AuthService.Application.Commands.Accounts;
using AuthService.Application.Commands.Registration;
using AuthService.Application.Queries.Accounts;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using EasyDesk.Tools.Options;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Web.Controllers.V_1_0.Accounts;

public class AccountController : AbstractMediatrController
{
    [HttpPost(AccountsRoutes.RegisterAccount)]
    public async Task<ActionResult<ResponseDto<AccountDto>>> RegisterAccount([FromBody] RegistrationBodyDto body)
    {
        var command = new RegisterAccount.Command(
            body.Email,
            body.Password,
            body.Username);
        return ForResponse(await Send(command))
            .Map(Mapper.Map<AccountDto>)
            .ReturnCreatedAtAction(nameof(GetAccount), x => new { accountId = x.Id });
    }

    [HttpGet(AccountsRoutes.GetAccount)]
    public async Task<ActionResult<ResponseDto<AccountDto>>> GetAccount([FromRoute] Guid accountId)
    {
        var query = new GetAccount.Query(accountId);
        return ForResponse(await Send(query))
            .Map(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpDelete(AccountsRoutes.DeleteAccount)]
    public async Task<ActionResult<ResponseDto<AccountDto>>> DeleteAccount([FromRoute] Guid accountId)
    {
        var command = new UnregisterAccount.Command(accountId);
        return ForResponse(await Send(command))
            .Map(Mapper.Map<AccountDto>)
            .ReturnOk();
    }

    [HttpPut(AccountsRoutes.ModifyAccount)]
    public async Task<ActionResult<ResponseDto<AccountDto>>> ModifyAccount([FromRoute] Guid accountId, [FromBody] ModifyAccountBodyDto body)
    {
        var command = new UpdateAccount.Command(accountId, body.Username.AsOption(), body.Email.AsOption());
        return ForResponse(await Send(command))
            .Map(Mapper.Map<AccountDto>)
            .ReturnOk();
    }
}
