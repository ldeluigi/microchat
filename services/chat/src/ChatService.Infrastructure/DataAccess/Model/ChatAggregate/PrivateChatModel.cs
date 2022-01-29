using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Model.ChatAggregate
{
    public class PrivateChatModel
    {
        public PrivateChatModel()
        {
        }

        /// <summary>
        /// The unique identifier of this chat.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the owner of this chat.
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        /// The unique identifier of the partecipant of this chat.
        /// </summary>
        public Guid Partecipant { get; set; }

        /// <summary>
        /// The dateTime when this chat is created.
        /// </summary>
        public Timestamp CreationTime { get; set; }

        public class Configuration : IEntityTypeConfiguration<PrivateChatModel>
        {
            public void Configure(EntityTypeBuilder<PrivateChatModel> builder)
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Owner)
                    .IsRequired();

                builder.Property(x => x.Partecipant)
                    .IsRequired();

                builder.Property(x => x.CreationTime)
                    .IsRequired();
            }
        }
    }
}
