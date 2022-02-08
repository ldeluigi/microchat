using EasyDesk.CleanArchitecture.Application.Authorization;
using System;

namespace UserService.Application;

public static class IUserInfoProviderExtensions
{
    public static Guid RequireUserId(this IUserInfoProvider userInfoProvider) =>
        new Guid(userInfoProvider.GetUserInfo().Value.UserId);
}
