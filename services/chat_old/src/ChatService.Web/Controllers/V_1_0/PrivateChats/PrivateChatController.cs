using ChatService.Application.Commands;
using ChatService.Application.Queries;
using ChatService.Web.Controllers.V_1_0.PrivateChats;
using ChatService.Web.Controllers.V_1_0.PrivateChats.DTO;
using ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0.DTO;

public class PrivateChatController : AbstractMediatrController
{
    [HttpPost(PrivateChatRoutes.RegisterPrivateChat)]
    public async Task<IActionResult> RegisterPrivateChat([FromBody] RegistrationBodyDto body)
    {
        var command = new RegisterPrivateChat.Command(
            body.ChatId,
            body.Owner,
            body.Partecipant);
        return await Command(command)
            .MappingContent(Mapper.Map<RegistrationBodyDto>)
            .ReturnOk();
    }

    [HttpGet(PrivateChatRoutes.GetPrivateChat)]
    public async Task<IActionResult> GetPrivateChat([FromRoute] Guid chatId)
    {
        var query = new GetPrivateChat.Query(chatId);
        return await Query(query)
            .MappingContent(Mapper.Map<PrivateChatDTO>)
            .ReturnOk();
    }

    [HttpGet(PrivateChatRoutes.GetUsersChats)]
    public async Task<IActionResult> GetPrivateChats(
        [FromQuery] Guid userId,
        [FromQuery] string pattern,
        [FromQuery] PaginationDto pagination)
    {
        var query = new GetPrivateChats.Query(userId, pattern, Mapper.Map<Pagination>(pagination));
        return await Query(query)
            .Paging(Mapper.Map<PrivateChatDTO>)
            .ReturnOk();
    }
}
