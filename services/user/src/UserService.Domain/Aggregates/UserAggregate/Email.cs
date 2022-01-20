using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Values;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace UserService.Domain.Aggregates.UserAggregate;

public record EmailEmptyError : DomainError;

public record EmailPatternError : DomainError;

public record Email : ValueWrapper<string, Email>
{
    private bool IsValid(string emailaddress)
    {
        try
        {
            var m = new MailAddress(emailaddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private Email(string email) : base(email)
    {
        DomainConstraints.Check()
            .If(string.IsNullOrWhiteSpace(email), () => new EmailEmptyError())
            .IfNot(IsValid(email), () => new EmailPatternError())
            .ThrowException();
    }

    public static Email From(string email) => new(email);
}
