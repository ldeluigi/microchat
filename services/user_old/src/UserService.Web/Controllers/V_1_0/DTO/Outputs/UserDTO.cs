using System;

namespace UserService.Web.Controllers.V_1_0.DTO.Outputs;

public record UserDto(
    Guid Id,
    string Name,
    string Surname,
    string Email);
