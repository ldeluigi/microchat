using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries;

/// <summary>
/// Query that retrieve some chats using pagination.
/// </summary>
public static partial class GetPrivateChats
{
    public record Query(
        string SearchString,
        Pagination Pagination) : PaginatedQueryBase<PrivateChatOutput>(Pagination);
}
