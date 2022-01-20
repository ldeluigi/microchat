using System;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace Microchat.UserService.Application.Queries;

public record UserSnapshot(
    Guid Id,
    string Name,
    string Email,
    string Password,
    Timestamp Creationtime,
    Option<Timestamp> LastLoginTime);
