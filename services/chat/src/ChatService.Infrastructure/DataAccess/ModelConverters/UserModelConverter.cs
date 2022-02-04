using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.Tools.Options;

namespace ChatService.Infrastructure.DataAccess.ModelConverters;

public class UserModelConverter : IModelConverter<User, UserModel>
{
    public void ApplyChanges(User origin, UserModel destination)
    {
        destination.Id = origin.Id;
        destination.LastSeenTime = origin.LastSeenTimestamp.OrElseNull();
    }

    public User ToDomain(UserModel model)
    {
        return new User(
            model.Id,
            model.LastSeenTime.AsOption());
    }
}
