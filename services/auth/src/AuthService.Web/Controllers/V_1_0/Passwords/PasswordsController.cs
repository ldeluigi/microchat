using AuthService.Application.Commands.Passwords;
using AuthService.Web.Controllers.V_1_0.Passwords.DTO;
using EasyDesk.CleanArchitecture.Application.UserInfo;
using EasyDesk.CleanArchitecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Passwords;

public class PasswordsController : AbstractMediatrController
{
    [HttpPut(PasswordsRoutes.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordBodyDto body, [FromServices] IUserInfo userInfo)
    {
        var command = new ChangePassword.Command(
            body.OldPassword,
            body.NewPassword,
            userInfo.UserId);
        return await Command(command)
            .ReturnOk();
    }

    [HttpPost(PasswordsRoutes.PasswordRecovery)]
    public async Task<IActionResult> PasswordRecovery([FromBody] PasswordRecoveryBodyDto body)
    {
        var command = new StartPasswordRecovery.Command(body.Email);
        return await Command(command)
            .ReturnOk();
    }

    [HttpPost(PasswordsRoutes.AcceptPasswordRecovery)]
    public async Task<IActionResult> AcceptPasswordRecovery([FromBody] AcceptPasswordRecoveryBodyDto body)
    {
        var command = new AcceptPasswordRecovery.Command(
            body.Token,
            body.NewPassword);
        return await Command(command)
            .ReturnOk();
    }
}
