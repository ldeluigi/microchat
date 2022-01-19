using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace AuthService.Application.Commands.Emails;

public static class ChangeEmail
{
    public record Command(Guid UserId, string Email) : CommandBase<Nothing>;

    public class Handler : UnitOfWorkHandler<Command, Nothing>
    {
        private readonly IAccountRepository _accountRepository;

        public Handler(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountRepository = accountRepository;
        }

        private Task<Response<Nothing>> HandlePrototype(Command request)
        {
            return OkAsync;
            ////var user = await _dataAccess
            ////    .Users
            ////    .Where(x => x.Id == request.UserId)
            ////    .Include(x => x.Identity)
            ////    .ThenInclude(x => x.AccountVerificationTokens)
            ////    .FirstAsync();

            ////if (user == null)
            ////{
            ////    return Errors.NotFound<User>();
            ////}

            ////var emailExists = await _dataAccess.Users
            ////    .Where(u => u.Email == request.Email)
            ////    .AnyAsync();

            ////if (emailExists)
            ////{
            ////    return Error.Create(IdentityErrorCodes.Email.AlreadyInUse, "The given email is already taken by another user");
            ////}

            ////var token = _tokenGenerator.NewAccountVerificationToken();

            ////user.Identity.AccountVerificationTokens.ForEach(t => t.Valid = false);

            ////user.Identity.AccountVerificationTokens.Add(token);

            ////user.EmailUpdate = request.Email;

            ////await _dataAccess.Save();

            ////await _publisher.Publish(new EmailChangeRequested
            ////{
            ////    NewEmail = request.Email,
            ////    OldEmail = user.Email,
            ////    Name = user.FirstName,
            ////    Token = token.Token
            ////});

            ////return new UserOutput
            ////{
            ////    Email = request.Email,
            ////    UserId = user.Id
            ////};
        }

        protected override Task<Response<Nothing>> HandleRequest(Command request)
        {
            throw new NotImplementedException();
        }
    }
}
