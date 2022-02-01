using ChatService.Application.Queries;
using ChatService.Domain.MessageService;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands;

public class DeleteMessage
{
    public record Command(Guid MessageId) : CommandBase<MessageOutput>;

    public class Handler : RequestHandlerBase<Command, MessageOutput>
    {
        private readonly MessageLifecycleService _messageLifecycleService;

        public Handler(MessageLifecycleService messageLifecycleService)
        {
            _messageLifecycleService = messageLifecycleService;
        }

        protected override async Task<Response<MessageOutput>> Handle(Command request)
        {
            return await _messageLifecycleService.Unregister(request.MessageId)
                .ThenMap(MessageOutput.From)
                .ThenToResponse();
        }
    }
}
