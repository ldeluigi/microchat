using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace ChatService.Application.Commands.PrivateMessages;

public class ViewPrivateMessage
{
    public record Command(Guid MessageId) : CommandBase<PrivateChatMessageOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUserInfoProvider _userInfoProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            IPrivateChatRepository privateChatRepository,
            IUserInfoProvider userInfoProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _privateChatRepository = privateChatRepository;
            _userInfoProvider = userInfoProvider;
        }

        protected override async Task<Response<PrivateChatMessageOutput>> Handle(Command request)
        {
            var userId = _userInfoProvider.RequireUserId();
            Response<PrivateChat> chatResult = default;
            return await _privateMessageRepository.GetById(request.MessageId)
                .ThenRequireAsync(async m => chatResult = await _privateChatRepository
                                                                    .GetById(m.ChatId)
                                                                    .ThenRequire(chat =>
                                                                    {
                                                                        if (!chat.PartecipantId.Contains(userId) && !chat.CreatorId.Contains(userId))
                                                                        {
                                                                            return Failure<PrivateChatMessageOutput>(new NotFoundError());
                                                                        }
                                                                        return Success(chat);
                                                                    }))
                .ThenIfSuccess(message => message.SetViewed())
                .ThenIfSuccess(message => _privateMessageRepository.Save(message))
                .ThenMap(m => PrivateChatMessageOutput.From(m, chatResult.ReadValue(), _userInfoProvider.RequireUserId()));
        }
    }
}
