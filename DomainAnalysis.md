# Domain Driven Design

## Ubiquitous Language

|Term | Definition|
|-----|-----------|
|Message| Piece of information sent in a chat by a user relative to a specific moment |
|Chat | Entity that contains messages and other metadata. A chat can involve at least one user.|
|User | Entity that uses the chat.|

## Context Map

```plantuml
@startuml Context Map
!include ContextMap.puml
@enduml
```

## Domain Analysis

### Chat domain
```plantuml
@startuml Chat Domain Map
!include Chat/Chat.Domain.puml
@enduml
```

#### Private Chat subdomain
```plantuml
@startuml PivateChat Subdomain Map
!include Chat/PrivateChat.Subdomain.puml
@enduml
```

#### Chat constraints

- *Chat* can't be deleted.
- *PrivateChat* can have only two partecipants, a owner and a partecipant.
- A *User* can interact with a *Chat* only if it is a participant.
- A *User* can send messages within a *Chat* only if it is a participant.
- A *User* can only delete messages in a *Chat* if it's the original sender.

### User domain
```plantuml
@startuml User Domain Map
!include User/User.Domain.puml
@enduml
```

#### User Constraints

