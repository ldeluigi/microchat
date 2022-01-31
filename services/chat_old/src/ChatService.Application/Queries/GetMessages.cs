using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries
{
    /// <summary>
    /// Query that retrieve some messages using pagination.
    /// </summary>
    public static partial class GetMessages
    {
        public record Query(
            Guid Sender,
            string SearchString,
            Pagination Pagination) : PaginatedQueryBase<MessageOutput>(Pagination);
    }
}
