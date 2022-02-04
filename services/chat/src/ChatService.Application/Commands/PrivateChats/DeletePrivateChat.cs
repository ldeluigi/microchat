using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateChats;

public class DeletePrivateChat
{
    public record Command(Guid ChatId) : CommandBase<PrivateChatOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatOutput>
    {
        private readonly IPrivateChatRepository _privateChatRepository;

        public Handler(
            IPrivateChatRepository privateChatRepository) => _privateChatRepository = privateChatRepository;

        protected override async Task<Response<PrivateChatOutput>> Handle(Command request) =>
                await _privateChatRepository
                .GetById(request.ChatId)
                .ThenIfSuccess(chat => _privateChatRepository.Remove(chat))
                .ThenMap(PrivateChatOutput.From)
                .ThenToResponse();
    }
}
