using AuthService.Domain.Authentication;
using EasyDesk.CleanArchitecture.Application.Mapping;

namespace AuthService.Web.Controllers.V_1_0.Tokens.DTO.Outputs;

public class AuthenticationResultMapping : SimpleMapping<AuthenticationResult, AuthenticationResultDto>
{
    public AuthenticationResultMapping()
    {
        AddCtorParam(nameof(AuthenticationResultDto.ExpirationDate), src => src.Expiration);
    }
}
