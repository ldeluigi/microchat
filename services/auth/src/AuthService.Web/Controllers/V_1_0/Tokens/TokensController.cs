using System.Threading.Tasks;
using AuthService.Application.Commands.Tokens;
using AuthService.Web.Controllers.V_1_0.Tokens.DTO;
using AuthService.Web.Controllers.V_1_0.Tokens.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Web.Controllers.V_1_0.Tokens;

public class TokensController : AbstractMediatrController
{
    [HttpPost(TokensRoutes.Login)]
    public async Task<ActionResult<ResponseDto<AuthenticationResultDto>>> Login([FromBody] LoginRequestDto body)
    {
        if (string.IsNullOrEmpty(body.Username))
        {
            var command = new LoginHandlers.EmailLoginCommand(body.Email, body.Password);
            return ForResponse(await Send(command))
                .Map(Mapper.Map<AuthenticationResultDto>)
                .ReturnOk();
        }
        else
        {
            var command = new LoginHandlers.UsernameLoginCommand(body.Username, body.Password);
            return ForResponse(await Send(command))
                .Map(Mapper.Map<AuthenticationResultDto>)
                .ReturnOk();
        }
    }

    [HttpPost(TokensRoutes.Refresh)]
    public async Task<ActionResult<ResponseDto<AuthenticationResultDto>>> RefreshToken([FromBody] RefreshTokenRequestDto body)
    {
        var command = new Refresh.Command(body.RefreshToken, body.AccessToken);
        return ForResponse(await Send(command))
            .Map(Mapper.Map<AuthenticationResultDto>)
            .ReturnOk();
    }
}
