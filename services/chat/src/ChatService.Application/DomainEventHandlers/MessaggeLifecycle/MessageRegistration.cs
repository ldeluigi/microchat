using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.DomainEventHandlers.MessaggeLifecycle;

public record MessageCreated(Guid Id, Guid ChatId, string Text, Guid Sender, Timestamp Sendtime) : DomainEvent;

public class MessageRegistration : DomainEventHandlerBase<MessageCreatedEvent>
{
    private readonly IMessageRepository _messageRepository;

    public MessageRegistration(IMessageRepository messageRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _messageRepository = messageRepository;
    }

    protected override Task<Response<Nothing>> Handle(MessageCreatedEvent ev) => throw new System.NotImplementedException();
}
