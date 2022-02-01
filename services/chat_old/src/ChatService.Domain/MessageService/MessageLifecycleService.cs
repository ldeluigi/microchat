using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Domain.MessageService;

public class MessageLifecycleService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IRegistrationMethod<MessageRegistrationData> _registrationMethod;

    public MessageLifecycleService(
        IMessageRepository messageRepository,
        MessageRegistrationMethod messageRegistrationMethod)
    {
        _messageRepository = messageRepository;
        _registrationMethod = messageRegistrationMethod;
    }

    public async Task<Result<Message>> Register(Guid id, Guid chatId, string text, Guid sender, Timestamp sendTime)
    {
        return await Task.FromResult(ResultImports.Ok)
            .ThenFlatMapAsync(_ => _registrationMethod.CreateMessage(
                new MessageRegistrationData(id, chatId, text, sender, sendTime))
            .ThenIfSuccess(message => _messageRepository.Save(message)));
    }

    public async Task<Result<Message>> Unregister(Guid messageId)
    {
        return await _messageRepository
            .GetById(messageId)
            .ThenIfSuccess(message => _messageRepository.Remove(message));
    }
}
