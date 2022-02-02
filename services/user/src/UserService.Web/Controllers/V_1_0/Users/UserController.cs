using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microchat.UserService.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Application.Queries;
using UserService.Web.Controllers.V_1_0.Users.DTO.Outputs;

namespace UserService.Web.Controllers.V_1_0.Users;

public class UserController : AbstractMediatrController
{
    [HttpGet(UserRoutes.GetUser)]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var query = new GetUser.Query(userId);
        return await Query(query)
            .MappingContent(Mapper.Map<UserDto>)
            .ReturnOk();
    }

    [HttpGet(UserRoutes.GetUsers)]
    public async Task<IActionResult> GetUsers([FromQuery] string search, [FromQuery] PaginationDto pagination)
    {
        var query = new GetUsers.Query(search, pagination);
        return await Query(query)
            .Paging(Mapper.Map<UserDto>)
            .ReturnOk();
    }
}
