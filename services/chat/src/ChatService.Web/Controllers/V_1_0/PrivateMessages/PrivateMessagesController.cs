using ChatService.Application.Queries.PrivateMessages;
using ChatService.Web.Controllers.V_1_0.PrivateMessages.DTO.Outputs;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0.PrivateMessages;

public class PrivateMessagesController : AbstractMediatrController
{
    [HttpGet(PrivateMessagesRoutes.PrivateMessages)]
    public async Task<ActionResult<ResponseDto<IEnumerable<PrivateChatMessageDto>>>> GetMessagesOfPrivateChat([FromQuery] Guid chatId, [FromQuery] PaginationDto pagination)
    {
        var query = new GetMessagesOfPrivateChat(chatId, pagination);
        return ForPageResponse(await Send(query))
            .MapEachElement(Mapper.Map<PrivateChatMessageDto>)
            .ReturnOk();
    }
}
