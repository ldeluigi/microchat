using ChatService.Application.Queries.PrivateChat;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Application.Commands.Chat;

public static class CreatePrivateChat
{
    public record Command(
       Guid Creator,
       Guid Partecipant) : CommandBase<PrivateChatOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatOutput>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly ITimestampProvider _timestampProvider;

        public Handler(
            IPrivateChatRepository privateChatRepository,
            ITimestampProvider timestampProvider)
        {
            _privateChatRepository = privateChatRepository;
            _timestampProvider = timestampProvider;
        }

        protected override async Task<Response<PrivateChatOutput>> Handle(Command request)
        {
            var chat = PrivateChat.Create(Guid.NewGuid(), request.Creator, request.Partecipant, _timestampProvider.Now);
            _privateChatRepository.Save(chat);
            return await Task.FromResult(Success(chat))
                .ThenMap(PrivateChatOutput.From)
                .ThenToResponse();
        }
    }
}
