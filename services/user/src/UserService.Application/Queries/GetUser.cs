using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace Microchat.UserService.Application.Queries;

/// <summary>
/// Request to get a specific User.
/// </summary>
public static class GetUser
{
    /// <summary>
    /// Find a User by his identifier.
    /// </summary>
    /// <param name="UserId">The user's identifier.</param>
    public record Query(Guid UserId) : QueryBase<UserSnapshot>;
}
