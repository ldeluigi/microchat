using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.Tools.Options;
using static EasyDesk.Tools.Options.OptionImports;

namespace UserService.Domain.Aggregates.UserAggregate;

public class User : AggregateRoot
{
    public Guid Id { get; }

    public Username Username { get; private set; }

    public Option<Name> Name { get; private set; }

    public Option<Name> Surname { get; private set; }

    public User(Guid id, Username username, Option<Name> name, Option<Name> surname)
    {
        Id = id;
        Username = username;
        Name = name;
        Surname = surname;
    }

    public void UpdateName(Name name) => Name = name;

    public void RemoveName() => Name = None;

    public void UpdateSurname(Name surname) => Surname = surname;

    public void RemoveSurname() => Surname = None;

    public void UpdateUsername(Username username) => Username = username;

    public static User Create(Guid id, Username username, Name name, Name surname) => new(id, username, name, surname);

    public static User Create(Guid id, Username username) => new(id, username, None, None);
}
