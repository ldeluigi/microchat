using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using System.Collections.Generic;
using System.Linq;

namespace ChatService.Infrastructure.DataAccess.Model.UserAggregate;

public static class UserModelExtensions
{
    public static IEnumerable<PrivateChatModel> PrivateChats(this UserModel userModel) =>
        userModel.PrivateChatCreated.Union(userModel.PrivateChatJoined);
}
