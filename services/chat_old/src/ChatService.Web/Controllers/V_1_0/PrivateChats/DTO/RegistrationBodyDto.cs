using System;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats.DTO;

public record RegistrationBodyDto(
    Guid ChatId,
    Guid Owner,
    Guid Partecipant);
