using AutoMapper;
using ChatService.Application.Queries;
using ChatService.Infrastructure.DataAccess.Repositories;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System;
using System.Threading.Tasks;
using static ChatService.Application.Queries.GetMessages;

namespace ChatService.Infrastructure.DataAccess.Queries;

public class GetMessaggesQueryHandle : PaginatedQueryHandlerBase<Query, MessageOutput>
{
    private readonly MessageContext _messageContext;
    private readonly IMapper _mapper;

    public GetMessaggesQueryHandle(MessageContext messageContext, IMapper mapper)
    {
        _messageContext = messageContext;
        _mapper = mapper;
    }

    protected override async Task<Response<Page<MessageOutput>>> Handle(Query request)
    {
        throw new NotImplementedException();
        /* return await _messageContext.Messages
            .Conditionally(request.Sender, id => query => query.Where(m => m.Sender == id))
            .OrderBy(m => m.CreationTime)
            .ProjectTo<MessageOutput>(_mapper.ConfigurationProvider)
            .GetPage(request.Pagination); */
    }
}
