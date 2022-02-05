using AuthService.Application.Commands.Passwords;
using AuthService.Web.Controllers.V_1_0.Accounts.DTO.Outputs;
using AuthService.Web.Controllers.V_1_0.Passwords.DTO;
using EasyDesk.CleanArchitecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Passwords;

public class PasswordController : AbstractMediatrController
{
    [HttpPut(PasswordsRoutes.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid accountId, [FromBody] ChangePasswordBodyDto body)
    {
        var command = new ChangePassword.Command(body.OldPassword, body.NewPassword, accountId);
        return await Command(command)
            .MappingContent(Mapper.Map<AccountDto>)
            .ReturnOk();
    }
}
