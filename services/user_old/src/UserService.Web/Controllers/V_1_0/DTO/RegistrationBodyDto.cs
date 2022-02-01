using System;

namespace UserService.Web.Controllers.V_1_0.DTO;

public record RegistrationBodyDto(
    Guid Id,
    string Name,
    string Surname,
    string Email);
