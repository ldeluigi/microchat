using AuthService.Domain.Tokens;
using EasyDesk.CleanArchitecture.Domain.Model;
using EasyDesk.Tools.Options;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public record EmailConfirmationInfo(Option<Email> NewEmail, TokenInfo Token);
