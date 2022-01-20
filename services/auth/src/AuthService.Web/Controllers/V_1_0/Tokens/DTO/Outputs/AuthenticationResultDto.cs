using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace AuthService.Web.Controllers.V_1_0.Tokens.DTO.Outputs;

public record AuthenticationResultDto(
    Guid UserId,
    string AccessToken,
    string RefreshToken,
    Timestamp ExpirationDate);
