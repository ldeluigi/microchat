using UserService.Domain.Aggregates.UserAggregate;
using UserService.Infrastructure.DataAccess.UserAggregate;

namespace UserService.Infrastructure.DataAccess.ModelConverter;

public class UserConverter
{
    public User ToDomain(UserModel model) =>
        new(
            id: model.Id,
            name: Name.From(model.Name),
            surname: Name.From(model.Surname),
            email: Email.From(model.Email));

    public void ApplyChanges(User origin, UserModel destination)
    {
        destination.Id = origin.Id;
        destination.Name = origin.Name;
        destination.Surname = origin.Surname;
        destination.Email = origin.Email;
    }
}
