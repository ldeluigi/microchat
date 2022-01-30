using ChatService.Domain.Aggregates.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;

namespace ChatService.Infrastructure.DataAccess.ModelConverter
{
    public class PrivateChatConverter
    {
        public PrivateChat ToDomain(PrivateChatModel model) => new(
        id: model.Id,
        owner: model.Owner,
        partecipant: model.Partecipant,
        creationTime: model.CreationTime);

        public void ApplyChanges(PrivateChat origin, PrivateChatModel destination)
        {
            destination.Id = origin.Id;
            destination.Owner = origin.Owner;
            destination.Partecipant = origin.Partecipant;
            destination.CreationTime = origin.CreationTime;
        }
    }
}
