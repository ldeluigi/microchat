using System;

namespace ChatService.Web.Controllers.V_1_0.Messages.DTO;

public record RegistrationBodyDto(
        Guid MessageId,
        Guid ChatId,
        string Text,
        Guid Sender);
