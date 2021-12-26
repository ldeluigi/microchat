using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using static EasyDesk.Tools.Options.OptionImports;

namespace Microchat.UserService.Domain.Aggregates.UserAggregate
{
    /// <summary>
    /// User class.
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// User constructor.
        /// </summary>
        /// <param name="id">User's identificator.</param>
        /// <param name="name">The name of this user.</param>
        /// <param name="email">The email of this user.</param>
        /// <param name="password">The password of this user's account.</param>
        /// <param name="creationTime">The time when this user has been created.</param>
        /// <param name="lastLoginTime">If present, the last time when login has been done.</param>
        public User(Guid id, string name, string email, string password, Timestamp creationTime, Option<Timestamp> lastLoginTime)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreationTime = creationTime;
            LastLoginTime = lastLoginTime;
        }

        /// <summary>
        /// A factory method for create a new User.
        /// </summary>
        /// <param name="name">The name of this user.</param>
        /// <param name="email">The email of this user.</param>
        /// <param name="password">The password of this user's account.</param>
        /// <param name="creationTime">The time when this user has been created.</param>
        /// <returns>The user with this parameters.</returns>
        public static User Create(string name, string email, string password, Timestamp creationTime)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (creationTime is null)
            {
                throw new ArgumentNullException(nameof(creationTime));
            }

            return new(Guid.NewGuid(), name, email, password, creationTime, None);
        }

        /// <summary>
        /// User's unique identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The user's email.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// The time when this user account has been created.
        /// </summary>
        public Timestamp CreationTime { get; }

        /// <summary>
        /// If present, the last time when login has been done.
        /// </summary>
        public Option<Timestamp> LastLoginTime { get; private set; }

        /// <summary>
        /// Change email of this user's account.
        /// </summary>
        /// <param name="newEmail">The new email of this user.</param>
        public void EditEmail(string newEmail)
        {
            LastLoginTime = Some(Timestamp.Now);
            Email = newEmail;
        }

        /// <summary>
        /// Change password of this user's account.
        /// </summary>
        /// <param name="newPassword">The new password of this user.</param>
        public void EditPassword(string newPassword)
        {
            LastLoginTime = Some(Timestamp.Now);
            Password = newPassword;
        }

        /// <summary>
        /// Change name of this user's account.
        /// </summary>
        /// <param name="newName">The new name of this user.</param>
        public void EditName(string newName)
        {
            LastLoginTime = Some(Timestamp.Now);
            Name = newName;
        }
    }
}
