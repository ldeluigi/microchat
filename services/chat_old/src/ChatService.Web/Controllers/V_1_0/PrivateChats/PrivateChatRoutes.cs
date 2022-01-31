namespace ChatService.Web.Controllers.V_1_0.PrivateChats;

public static class PrivateChatRoutes
{
    public const string RegisterPrivateChat = "chats";

    public const string GetUsersChats = "users/chats/{userId}";

    public const string GetPrivateChat = "privatechats/{privateChatId}";
}
