using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using Microchat.UserService.Application.Queries;
using Microchat.UserService.Domain.Aggregates.UserAggregate;

namespace Microchat.ChatService.Application.Commands
{
    /// <summary>
    /// Request of create user.
    /// </summary>
    public static class CreateUser
    {
        /// <summary>
        /// The command data for the <see cref="CreateUser"/> command.
        /// <param name="Name">The name of this user.</param>
        /// <param name="Email">The email of this user.</param>
        /// <param name="Password">The password of this user's account.</param>
        /// </summary>
        public record Command(string Name, string Email, string Password) : CommandBase<UserSnapshot>;

        /// <summary>
        /// The handler for the <see cref="CreateUser"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, UserSnapshot>
        {
            private readonly IUserRepository _userRepository;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="chatRepository">The chat repository.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                IUserRepository chatRepository,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _userRepository = chatRepository;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected override Task<Response<UserSnapshot>> HandleRequest(Command request)
            {
                var user = User.Create(request.Name, request.Email, request.Password, _timestampProvider.Now);
                _userRepository.Save(user);
                return Task.FromResult<Response<UserSnapshot>>(new UserSnapshot(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Password,
                    user.CreationTime,
                    user.LastLoginTime));
            }
        }
    }
}
