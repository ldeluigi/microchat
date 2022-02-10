using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using EasyDesk.Tools.Options;
using Microchat.UserService.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.Commands;
using UserService.Application.Queries;
using UserService.Web.Controllers.V_1_0.Users.DTO.Outputs;

namespace UserService.Web.Controllers.V_1_0.Users;

public class UserController : AbstractMediatrController
{
    [HttpGet(UserRoutes.GetUser)]
    public async Task<ActionResult<ResponseDto<UserDto>>> GetUser([FromRoute] Guid userId)
    {
        var query = new GetUser.Query(userId);
        return ForResponse(await Send(query))
            .Map(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpGet(UserRoutes.GetUsers)]
    public async Task<ActionResult<ResponseDto<IEnumerable<UserDto>>>> GetUsers([FromQuery] string search, [FromQuery] PaginationDto pagination)
    {
        var query = new GetUsers.Query(search, pagination);
        return ForPageResponse(await Send(query))
            .MapEachElement(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpPut(UserRoutes.UpdateUser)]
    public async Task<ActionResult<ResponseDto<UserDto>>> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserBodyDTO body)
    {
        var command = new UpdateUser.Command(userId, body.Name.AsOption(), body.Surname.AsOption());
        return ForResponse(await Send(command))
            .Map(Mapper.Map<UserDto>)
            .ReturnOk();
    }
}
