﻿using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
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
            Result<PrivateChat> chatResult = default;
            return await _privateMessageRepository
                .GetById(request.MessageId)
                .ThenRequireAsync(async m => chatResult = await _privateChatRepository.GetById(m.ChatId))
                .ThenIfSuccess(request => _privateMessageRepository.Remove(request))
                .ThenMap(m => PrivateChatMessageOutput.From(m, chatResult.ReadValue(), _userInfoProvider.RequireUserId()))
                .ThenToResponse();
        }
    }
}
