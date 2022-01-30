using System;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;

public record PrivateChatDTO(
    Guid Id,
    Guid Owner,
    Guid Partecipant);
