using AuthService.Application.Commands.Passwords;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;
using AuthService.Web.Controllers.V_1_0.Passwords.DTO;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Passwords;

public class PasswordController : AbstractMediatrController
{
    [HttpPut(PasswordsRoutes.ChangePassword)]
    public async Task<ActionResult<ResponseDto<AccountDto>>> ChangePassword([FromRoute] Guid accountId, [FromBody] ChangePasswordBodyDto body)
    {
        var command = new ChangePassword.Command(body.OldPassword, body.NewPassword, accountId);
        return ForResponse(await Send(command))
            .Map(Mapper.Map<AccountDto>)
            .ReturnOk();
    }
}
