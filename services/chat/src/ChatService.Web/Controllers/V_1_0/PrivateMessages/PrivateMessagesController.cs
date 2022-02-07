using ChatService.Application.Queries.PrivateMessages;
using ChatService.Web.Controllers.V_1_0.PrivateMessages.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0.PrivateMessages;

public class PrivateMessagesController : AbstractMediatrController
{
    [HttpGet(PrivateMessagesRoutes.PrivateMessages)]
    public async Task<IActionResult> GetMessagesOfPrivateChat([FromQuery] Guid chatId, [FromQuery] PaginationDto pagination)
    {
        var query = new GetMessagesOfPrivateChat(chatId, pagination);
        return await Query(query)
            .MappingContent(Mapper.Map<PrivateChatMessageDto>)
            .ReturnOk();
    }
}
