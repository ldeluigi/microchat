# User Subdomain Model

## User context
```plantuml
@startuml User Domain User Context
!include meta/domain-analysis.metamodel.iuml

$aggregate(User) {
    $aggregate_root(User) {
        + id: Guid
        + username: Username
        + name: Name
        + surname: Name
        + updateName(Name)
        + updateSurname(Name)
        + updateUsername(Username)
    }

    $value(Name) {
        + value: String
    }

    $value(Username) {
        + value: String
    }

    
    Name --o User
    Username --o User
}

@enduml
```
### Details

#### Name and Surname

**constraints**:

- $value$ must have almost 4 letters.
- $value$ can't have more than 100 letters.

#### Username

**constraints**:

- $value$ must have almost 4 letters.
- $value$ matches `^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$`

## Auth context
```plantuml
@startuml User Domain Auth Context
!include meta/domain-analysis.metamodel.iuml

$aggregate(Account) {
    $aggregate_root(Account) {
        + id: Guid
        + creation: Timestamp
        + username: Username
        + email: Email
        + passwordHash: passwordHash
        + sessions: AccountSessions
    }

    $value(Username) {
        + value: String
    }

    $value(Email) {
        + value: String
    }

    $value(PasswordHash) {
        + password: String
        + salt: String
    }

    $value(AccountSessions) {
        + sessions: Set<Session> 
    }

    $value(Session) {
        + accessTokenId: Guid
        + expiration: Timestamp
        + refreshToken: Token
    }

    Username -o Account
    Email --o Account
    PasswordHash --o Account
    AccountSessions --o Account
    Session --o AccountSessions
}
@enduml
```

### Details

#### Username

**constraints**:

- $value$ must have almost 4 letters.
- $value$ matches `^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$`

#### Email

**constraints**:

- $value$ must respect regex for email.

#### Password Hash

**constraints**:

- $password$ must not be empty.
- $password$ must have almost 8 characters.

### Domain Events

* **Account Registered**: emitted when an account is registered to the system.
* **Account Unregistered**: emitted when an account is removed from the system.
