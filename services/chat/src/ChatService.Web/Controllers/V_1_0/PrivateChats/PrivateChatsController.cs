using ChatService.Application.Queries.PrivateChats;
using ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats;

public class PrivateChatsController : AbstractMediatrController
{
    [HttpGet(PrivateChatsRoutes.PrivateChats)]
    public async Task<ActionResult<ResponseDto<IEnumerable<PrivateChatOfUserDto>>>> GetPrivateChatsOfUser([FromQuery] Guid userId, [FromQuery] PaginationDto pagination)
    {
        var query = new GetPrivateChatsOfUser(userId, pagination);
        return ForPageResponse(await Send(query))
            .MapEachElement(Mapper.Map<PrivateChatOfUserDto>)
            .ReturnOk();
    }

    [HttpGet(PrivateChatsRoutes.PrivateChat)]
    public async Task<ActionResult<ResponseDto<PrivateChatDetailsDto>>> GetPrivateChatDetails([FromRoute] Guid chatId)
    {
        var query = new GetPrivateChatDetails(chatId);
        return ForResponse(await Send(query))
            .Map(Mapper.Map<PrivateChatDetailsDto>)
            .ReturnOk();
    }
}
