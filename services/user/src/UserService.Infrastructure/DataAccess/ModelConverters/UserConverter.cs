using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.Tools.Options;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Infrastructure.DataAccess.Model.UserAggregate;

namespace UserService.Infrastructure.DataAccess.ModelConverters;

public class UserConverter : IModelConverter<User, UserModel>
{
    public void ApplyChanges(User origin, UserModel destination)
    {
        destination.Id = origin.Id;
        destination.Name = origin.Name
            .Map(x => x.Value) | string.Empty;
        destination.Surname = origin.Surname
            .Map(x => x.Value) | string.Empty;
        destination.Username = origin.Username;
    }

    public User ToDomain(UserModel model)
    {
        return new User(
            id: model.Id,
            username: Username.From(model.Username),
            name: Name.From(model.Name),
            surname: Name.From(model.Surname));
    }
}
