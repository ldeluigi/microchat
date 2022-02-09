# User Data Domain Model

## User context
```plantuml
@startuml User Domain Map
!include domain-analysis/domain-models/user/user-domain-user-context.puml
@enduml
```
### Details

#### Name and Surname

**constraints**:

- $value must have almost 4 letters.
- $value can't have more than 100 letters.

#### Username

**constraints**:

- $value must have almost 4 letters.
- value matches `^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$`

## Auth context
```plantuml
@startuml Auth Domain Map
!include domain-analysis/domain-models/user/user-domain-auth-context.puml
@enduml
```

### Details

#### Username

**constraints**:

- $value must have almost 4 letters.
- $value matches `^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$`

#### Email

**constraints**:

- $value must respect regex for email.

#### Password Hash

**constraints**:

- $password must not be empty.
- $password must have almost 8 characters.

### Domain Events

* **Account created**: emitted when an account is registered to the system.
* **Account Unregistered**: emitted when an account is removed from the system.