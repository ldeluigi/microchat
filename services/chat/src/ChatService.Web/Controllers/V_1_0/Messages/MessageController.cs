using System;
using System.Threading.Tasks;
using ChatService.Application.Commands;
using ChatService.Web.Controllers.V_1_0.Messages.DTO;
using ChatService.Web.Controllers.V_1_0.Messages.DTO.Outputs;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Web.Controllers;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Web.Controllers.V_1_0.Messages;

public class MessageController : AbstractMediatrController
{
    [HttpPost(MessageRoutes.SendMessage)]
    public async Task<IActionResult> SendMessage([FromBody] RegistrationBodyDto body)
    {
        var command = new SendMessage.Command(
            body.MessageId,
            body.ChatId,
            body.Text,
            body.Sender);
        return await Command(command)
            .MappingContent(Mapper.Map<RegistrationBodyDto>)
            .ReturnOk();
    }

    [HttpDelete(MessageRoutes.DeleteMessage)]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
    {
        var command = new DeleteMessage.Command(messageId);
        return await Command(command)
            .MappingContent(Mapper.Map<MessageDTO>)
            .ReturnOk();
    }

    [HttpDelete(MessageRoutes.ModifyMessage)]
    public async Task<IActionResult> ModifyMessage([FromRoute] Guid messageId, [FromBody] ModifyAccountBodyDto body)
    {
        var command = new EditMessage.Command(messageId, body.Text);
        return await Command(command)
            .ReturnOk();
    }

    [HttpGet(MessageRoutes.GetMessages)]
    public async Task<IActionResult> GetMessages([FromQuery] string pattern, [FromQuery] PaginationDto pagination)
    {
        var query = new Application.Queries.GetMessages.Query(pattern, Mapper.Map<Pagination>(pagination));
        return await Query(query)
            .Paging(Mapper.Map<MessageDTO>)
            .ReturnOk();
    }
}
