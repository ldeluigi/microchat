using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using System;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats.Mappers;

public static class PrivateChatModelMapper
{
    public static PrivateChatOfUserOutput ConvertModelToOutput(PrivateChatModel privateChat, int unreadMessages, Guid asSeenBy)
    {
        var isParticipant = asSeenBy == privateChat.CreatorId || asSeenBy == privateChat.PartecipantId;

        return new(
            Id: privateChat.Id,
            CreatorId: privateChat.CreatorId.AsOption(),
            PartecipantId: privateChat.PartecipantId.AsOption(),
            CreationTimestamp: privateChat.CreationTime,
            NumberOfUnreadMessages: isParticipant ? Some(unreadMessages) : None);
    }
}
