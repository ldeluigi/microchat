using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateMessages;

public class DeletePrivateMessage
{
    public record Command(Guid MessageId) : CommandBase<PrivateChatMessageOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IUserInfoProvider _userInfoProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            IUserInfoProvider userInfoProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _userInfoProvider = userInfoProvider;
        }

        protected override async Task<Response<PrivateChatMessageOutput>> Handle(Command request) =>
                await _privateMessageRepository
                .GetById(request.MessageId)
                .ThenIfSuccess(request => _privateMessageRepository.Remove(request))
                .ThenMap(m => PrivateChatMessageOutput.From(m, _userInfoProvider.RequireUserId()))
                .ThenToResponse();
    }
}
