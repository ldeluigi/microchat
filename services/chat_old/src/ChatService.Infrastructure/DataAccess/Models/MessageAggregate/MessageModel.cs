using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Model.MessageAggregate;

public class MessageModel
{
    /// <summary>
    /// The unique identifier of this message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of this message's chat.
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// The text of this message.
    /// </summary>
    public string Text { get; set; }

    public Guid Sender { get; set; }

    /// <summary>
    /// The time when the message have been sent for the first time.
    /// </summary>
    public Timestamp SendTime { get; set; }

    /// <summary>
    /// Last time when the message have been modified.
    /// </summary>
    public Option<Timestamp> LastEditTime { get; set; }
}
