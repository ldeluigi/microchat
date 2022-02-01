using System;
using EasyDesk.CleanArchitecture.Domain.Metamodel;

namespace UserService.Domain.Aggregates.UserAggregate;

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
    /// <param name="surname">The surname of this user.</param>
    /// <param name="email">The email of this user.</param>
    public User(Guid id, Name name, Name surname, Email email)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
    }

    /// <summary>
    /// A factory method for create a new User.
    /// </summary>
    /// <param name="id">The id of this user.</param>
    /// <param name="name">The name of this user.</param>
    /// <param name="surname">The surname of this user.</param>
    /// <param name="email">The email of this user.</param>
    /// <returns>The user with this parameters.</returns>
    public static User Create(Guid id, Name name, Name surname, Email email) => new(id, name, surname, email);

    public static User Create(Guid id)
    {
        return new(id,
            Name.From("Name"),
            Name.From("Surname"),
            Email.From("defaultemail@default.com"));
    }

    /// <summary>
    /// User's unique identifier.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The user's name.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// The user's surname.
    /// </summary>
    public Name Surname { get; private set; }

    /// <summary>
    /// The user's email.
    /// </summary>
    public Email Email { get; private set; }

    public void UpdateEmail(Email email) => Email = email;

    public void UpdateName(Name name) => Name = name;

    public void UpdateSurname(Name surname) => Surname = surname;
}
