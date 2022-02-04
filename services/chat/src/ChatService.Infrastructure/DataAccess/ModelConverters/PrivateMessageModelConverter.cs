using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.Tools.Options;

namespace ChatService.Infrastructure.DataAccess.ModelConverters;

public class PrivateMessageModelConverter : IModelConverter<PrivateMessage, PrivateMessageModel>
{
    public void ApplyChanges(PrivateMessage origin, PrivateMessageModel destination)
    {
        destination.Id = origin.Id;
        destination.SenderId = origin.SenderId.AsNullable();
        destination.Viewed = origin.Viewed;
        destination.Text = origin.Text;
        destination.LastEditTime = origin.LastEditTime.OrElseNull();
        destination.SendTime = origin.SendTime;
        destination.ChatId = origin.ChatId;
    }

    public PrivateMessage ToDomain(PrivateMessageModel model)
    {
        return new PrivateMessage(
            model.Id,
            model.ChatId,
            MessageText.From(model.Text),
            model.SenderId.AsOption(),
            model.SendTime,
            model.LastEditTime.AsOption(),
            model.Viewed);
    }
}
