using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using EasyDesk.Tools.Options;
using Microchat.UserService.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Application.Commands;
using UserService.Application.Queries;
using UserService.Web.Controllers.V_1_0.DTO;
using UserService.Web.Controllers.V_1_0.DTO.Outputs;

namespace UserService.Web.Controllers.V_1_0.User;

public class UserController : AbstractMediatrController
{
    [HttpPost(UsersRoutes.RegisterUser)]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationBodyDto body)
    {
        var command = new RegisterUser.Command(
            body.Id,
            body.Name,
            body.Surname,
            body.Email);
        return await Command(command)
            .MappingContent(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpGet(UsersRoutes.GetUser)]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var query = new GetUser.Query(userId);
        return await Query(query)
            .MappingContent(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpGet(UsersRoutes.GetUsers)]
    public async Task<IActionResult> GetAccounts([FromQuery] string pattern, [FromQuery] PaginationDto pagination)
    {
        var query = new GetUsers.Query(pattern, Mapper.Map<Pagination>(pagination));
        return await Query(query)
            .Paging(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpDelete(UsersRoutes.DeleteUser)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid accountId)
    {
        var command = new UnregisterUser.Command(accountId);
        return await Command(command)
            .MappingContent(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpPut(UsersRoutes.ModifyUser)]
    public async Task<IActionResult> ModifyAccount([FromRoute] Guid userId, [FromBody] ModifyUserBodyDto body)
    {
        var command = new UpdateUser.Command(userId, body.Name.AsOption(), body.Surname.AsOption(), body.Email.AsOption());
        return await Command(command)
            .MappingContent(Mapper.Map<UserDto>)
            .ReturnOk();
    }
}
