using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;

namespace ChatService.Application.Queries
{
    /// <summary>
    /// Query that retrieve some messages using pagination.
    /// </summary>
    public static partial class GetMessages
    {
        public record Query(
            string SearchString,
            Pagination Pagination) : PaginatedQueryBase<MessageOutput>(Pagination);
    }
}
