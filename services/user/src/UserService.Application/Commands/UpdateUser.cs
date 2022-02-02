using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.Options;
using FluentValidation;
using Microchat.UserService.Application.Queries;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.Commands;

public static class UpdateUser
{
    public record Command(Guid UserId, Option<string> Name, Option<string> Surname) : CommandBase<UserOutput>;

    public class Validator : AbstractValidator<Command>
    {
        private void ValidateName(Func<Command, Option<string>> field) =>
            When(c => field(c).IsPresent, () =>
                RuleFor(c => field(c).Value)
                    .Length(Name.MinimumLength, Name.MaximumLength));

        public Validator()
        {
            ValidateName(c => c.Name);
            ValidateName(c => c.Surname);
        }
    }

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
                .ThenIfSuccess(user =>
                {
                    if (request.Name.IsPresent)
                    {
                        user.UpdateName(Name.From(request.Name.Value));
                    }
                    if (request.Surname.IsPresent)
                    {
                        user.UpdateSurname(Name.From(request.Surname.Value));
                    }
                })
                .ThenIfSuccess(user => _userRepository.Save(user))
                .ThenMap(UserOutput.From)
                .ThenToResponse();
        }
    }
}
