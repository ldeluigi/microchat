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
        destination.Name = origin.Name.OrElseNull();
        destination.Surname = origin.Surname.OrElseNull();
        destination.Username = origin.Username;
    }

    public User ToDomain(UserModel model)
    {
        return new User(
            id: model.Id,
            username: Username.From(model.Username),
            name: model.Name.AsOption().Map(Name.From),
            surname: model.Surname.AsOption().Map(Name.From));
    }
}
