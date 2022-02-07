using ChatService.Application.Queries.PrivateChats.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatService.Web.Controllers.V_1_0.SignalRChatDtos;

public static class PrivateChatOutputExtensions
{
    public static IEnumerable<string> MembersIds(this PrivateChatOutput privateChatOutput) =>
        privateChatOutput.Members.Select(g => g.ToString());

    public static IEnumerable<string> OtherMembersIds(this PrivateChatOutput privateChatOutput, Guid me) =>
        privateChatOutput.Members.Where(g => g != me).Select(g => g.ToString());
}
