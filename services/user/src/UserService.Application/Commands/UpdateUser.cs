using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.Options;
using Microchat.UserService.Application.Queries;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.Commands;

// farsi passare id e poi modificare i campi modificabili (opzionali perchè non necessariamente li voglio modificare)

// carico user con user repo
// modifico i campi
// salvo lo user aggiornato facendo thenIfSuccess per ogni modifica e per ultimo thenToResponse()
// simile a AuthService changePassword
public static class UpdateUser
{
    public record Command(Guid UserId, Option<string> Name, Option<string> Surname, Option<string> Email) : CommandBase<UserOutput>;

    public class Handler : RequestHandlerBase<Command, UserOutput>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task<Response<UserOutput>> Handle(Command request)
        {
            return await _userRepository.GetById(request.UserId)
                .ThenIfSuccess(user => request.Name.IfPresent(userName => user.UpdateName(Name.From(userName))))
                .ThenIfSuccess(user => request.Surname.IfPresent(userSurname => user.UpdateSurname(Name.From(userSurname))))
                .ThenIfSuccess(user => request.Email.IfPresent(userEmail => user.UpdateEmail(Email.From(userEmail))))
                .ThenIfSuccess(user => _userRepository.Save(user))
                .ThenMap(UserOutput.From)
                .ThenToResponse();
        }
    }
}
