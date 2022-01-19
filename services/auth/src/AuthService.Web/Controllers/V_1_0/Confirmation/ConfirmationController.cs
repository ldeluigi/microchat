using AuthService.Application.Commands.Emails;
using AuthService.Web.Controllers.V_1_0.Confirmation.DTO;
using EasyDesk.CleanArchitecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Confirmation;

public class ConfirmationController : AbstractMediatrController
{
    [HttpPost(ConfirmationRoutes.ConfirmAccount)]
    public async Task<IActionResult> ConfirmAccount([FromBody] AccountConfirmationBodyDto body)
    {
        var command = new ConfirmEmail.Command(body.Token);
        return await Command(command)
            .ReturnOk();
    }

    [HttpPost(ConfirmationRoutes.SendAccountVerification)]
    public async Task<IActionResult> SendAccountVerification([FromBody] SendVerificationBodyDto body)
    {
        var command = new RegenerateEmailToken.Command(body.Email);
        return await Command(command)
            .ReturnOk();
    }
}
