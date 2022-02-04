using ChatService.Domain.Aggregates.PrivateChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.Tools.Options;

namespace ChatService.Infrastructure.DataAccess.ModelConverters;

public class PrivateChatModelConverter : IModelConverter<PrivateChat, PrivateChatModel>
{
    public void ApplyChanges(PrivateChat origin, PrivateChatModel destination)
    {
        destination.Id = origin.Id;
        destination.PartecipantId = origin.PartecipantId.AsNullable();
        destination.CreatorId = origin.CreatorId.AsNullable();
        destination.CreationTime = origin.CreationTime;
    }

    public PrivateChat ToDomain(PrivateChatModel model)
    {
        return new PrivateChat(
            id: model.Id,
            creator: model.CreatorId.AsOption(),
            partecipant: model.PartecipantId.AsOption(),
            creationTime: model.CreationTime);
    }
}
