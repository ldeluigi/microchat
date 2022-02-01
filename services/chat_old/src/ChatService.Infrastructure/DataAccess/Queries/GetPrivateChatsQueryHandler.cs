using AutoMapper;
using ChatService.Application.Queries;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using System;
using System.Threading.Tasks;
using static ChatService.Application.Queries.GetPrivateChats;

namespace ChatService.Infrastructure.DataAccess.Queries;

public class GetPrivateChatsQueryHandler : PaginatedQueryHandlerBase<Query, PrivateChatOutput>
{
    private readonly PrivateChatContext _chatContext;
    private readonly IMapper _mapper;

    public GetPrivateChatsQueryHandler(PrivateChatContext chatContext, IMapper mapper)
    {
        _chatContext = chatContext;
        _mapper = mapper;
    }

    public class PrivateChatModelMapping : DirectMapping<PrivateChatModel, PrivateChatOutput>
    {
        public PrivateChatModelMapping() : base(src => new(
            src.Id,
            src.Owner,
            src.Partecipant,
            src.CreationTime))
        {
        }
    }

    // TODO
    protected override async Task<Response<Page<PrivateChatOutput>>> Handle(Query request)
    {
        throw new NotImplementedException();
        /*return await _chatContext.PrivateChats
            .Conditionally(request.UserId, id => query => query.Where(c => (c.owner == id || c.partecipant == id))
            .OrderBy(r => r.RequestTimestamp)
            .ProjectTo<PrivateChatOutput>(_mapper.ConfigurationProvider)
            .GetPage(request.Pagination);*/
    }
}
