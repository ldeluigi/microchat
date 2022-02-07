using ChatService.Domain;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace ChatService.Application.DomainEvents;

public class UserDeletedHandler : DomainEventHandlerBase<UserDeleted>
{
    private readonly IPrivateChatRepository _privateChatRepository;

    public UserDeletedHandler(IPrivateChatRepository privateChatRepository)
    {
        _privateChatRepository = privateChatRepository;
    }

    protected override Task<Response<Nothing>> Handle(UserDeleted ev)
    {
        return OkAsync;
    }
}
