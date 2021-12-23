using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
namespace Microchat.Domain.Aggregates.User
{
    public class User : AggregateRoot
    {

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;

        }

        public Guid Id { get; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public DateTime CreationTime { get; }

        public Option<DateTime> LastLoginTime { get; private set; }


    }
}
