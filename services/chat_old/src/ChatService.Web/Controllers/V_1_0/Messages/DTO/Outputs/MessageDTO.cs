using System;

namespace ChatService.Web.Controllers.V_1_0.Messages.DTO.Outputs;
public record MessageDTO(
        Guid Id,
        Guid ChatId,
        string Text,
        Guid Sender);
