using ChatService.Application.Queries.PrivateChats;
using ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats;

public class PrivateChatsController : AbstractMediatrController
{
    [HttpGet(PrivateChatsRoutes.PrivateChats)]
    public async Task<IActionResult> GetPrivateChatsOfUser([FromQuery] Guid userId, [FromQuery] PaginationDto pagination)
    {
        var query = new GetPrivateChatsOfUser(userId, pagination);
        return await Query(query)
            .MappingPageContent(Mapper.Map<PrivateChatOfUserDto>)
            .ReturnOk();
    }

    [HttpGet(PrivateChatsRoutes.PrivateChat)]
    public async Task<IActionResult> GetPrivateChatDetails([FromRoute] Guid chatId)
    {
        var query = new GetPrivateChatDetails(chatId);
        return await Query(query)
            .MappingContent(Mapper.Map<PrivateChatDetailsDto>)
            .ReturnOk();
    }
}
