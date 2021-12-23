using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace Microchat.ChatService.Domain.Aggregates.UserAggregate
{
    /// <summary>
    /// User entity.
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// Creation of an User.
        /// </summary>
        /// <param name="id">Identificator of the User.</param>
        public User(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Identificator of this User.
        /// </summary>
        public Guid Id { get; }
    }
}
