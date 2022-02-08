using System;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using FluentValidation;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Application.Commands.PrivateMessages;

public class SendPrivateMessage
{
    public record Command(
        Guid ChatId,
        string Text) : CommandBase<PrivateChatMessageOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Text)
                .MaximumLength(MessageText.MaximumLength)
                .NotEmpty();
        }
    }

    public class Handler : RequestHandlerBase<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly ITimestampProvider _timestampProvider;
        private readonly IUserInfoProvider _userInfoProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            IPrivateChatRepository privateChatRepository,
            ITimestampProvider timestampProvider,
            IUserInfoProvider userInfoProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _privateChatRepository = privateChatRepository;
            _timestampProvider = timestampProvider;
            _userInfoProvider = userInfoProvider;
        }

        protected override Task<Response<PrivateChatMessageOutput>> Handle(Command request)
        {
            var userId = _userInfoProvider.RequireUserId();
            var message = PrivateMessage.Create(
                id: Guid.NewGuid(),
                chatId: request.ChatId,
                text: MessageText.From(request.Text),
                senderId: userId,
                sendTime: _timestampProvider.Now);
            return _privateChatRepository
                .GetById(message.ChatId)
                .ThenRequire(chat =>
                {
                    if (!chat.PartecipantId.Contains(userId) && !chat.CreatorId.Contains(userId))
                    {
                        return Failure<PrivateChatMessageOutput>(new NotFoundError());
                    }
                    return Success(chat);
                })
                .ThenIfSuccess(_ => _privateMessageRepository.Save(message))
                .ThenMap(c => PrivateChatMessageOutput.From(message, c, userId));
        }
    }
}
