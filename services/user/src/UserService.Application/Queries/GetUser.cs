using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace Microchat.UserService.Application.Queries;

public static class GetUser
{
    public record Query(Guid UserId) : QueryBase<UserOutput>;
}
