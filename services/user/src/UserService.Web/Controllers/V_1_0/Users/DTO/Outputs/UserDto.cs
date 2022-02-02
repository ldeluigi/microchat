using EasyDesk.CleanArchitecture.Application.Mapping;
using Microchat.UserService.Application.Queries;
using System;

namespace UserService.Web.Controllers.V_1_0.Users.DTO.Outputs;

public record UserDto(
    Guid Id,
    string Username,
    string Name,
    string Surname);

public class UserOutputMapping : SimpleMapping<UserOutput, UserDto>
{
}
