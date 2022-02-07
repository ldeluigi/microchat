using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatDetailsHandler : RequestHandlerBase<GetPrivateChatDetails, DetailedPrivateChatOutput>
{
    private readonly ChatContext _chatContext;

    public GetPrivateChatDetailsHandler(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    protected override async Task<Response<DetailedPrivateChatOutput>> Handle(GetPrivateChatDetails request) =>
        await _chatContext.PrivateChats
            .Where(c => c.Id == request.Id)
            .GroupJoin(_chatContext.PrivateMessages, on => on.Id, on => on.ChatId, (chat, messages) =>
                new DetailedPrivateChatOutput(
                    chat.Id,
                    chat.CreatorId.AsOption(),
                    chat.PartecipantId.AsOption(),
                    chat.CreationTime,
                    messages.Count()))
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
}
