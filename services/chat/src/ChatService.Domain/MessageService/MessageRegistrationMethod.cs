using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Domain.MessageService;

public record MessageRegistrationData(Guid Id, Guid ChatId, string Text, Guid Sender, Timestamp SendTime);

public class MessageRegistrationMethod : IRegistrationMethod<MessageRegistrationData>
{
    private readonly IMessageRepository _userRepository;

    public MessageRegistrationMethod(IMessageRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public Task<Result<Message>> CreateMessage(MessageRegistrationData messageData)
    {
        var message = Message.Create(
            messageData.Id,
            messageData.ChatId,
            messageData.Text,
            messageData.Sender,
            messageData.SendTime);

        _userRepository.Save(message);

        return Task.FromResult(Success(message));
    }
}
