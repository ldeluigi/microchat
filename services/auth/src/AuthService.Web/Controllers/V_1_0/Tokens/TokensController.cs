using AuthService.Application.Commands.Tokens;
using AuthService.Web.Controllers.V_1_0.Tokens.DTO;
using AuthService.Web.Controllers.V_1_0.Tokens.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Web.Controllers.V_1_0.Tokens;

public class TokensController : AbstractMediatrController
{
    [HttpPost(TokensRoutes.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto body)
    {
        if (string.IsNullOrEmpty(body.Username))
        {
            var command = new LoginHandlers.EmailLoginCommand(body.Email, body.Password);
            return await Command(command)
                .MappingContent(Mapper.Map<AuthenticationResultDto>)
                .ReturnOk();
        }
        else
        {
            var command = new LoginHandlers.UsernameLoginCommand(body.Username, body.Password);
            return await Command(command)
                .MappingContent(Mapper.Map<AuthenticationResultDto>)
                .ReturnOk();
        }
    }

    [HttpPost(TokensRoutes.Refresh)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto body)
    {
        var command = new Refresh.Command(body.RefreshToken, body.AccessToken);
        return await Command(command)
            .MappingContent(Mapper.Map<AuthenticationResultDto>)
            .ReturnOk();
    }
}
