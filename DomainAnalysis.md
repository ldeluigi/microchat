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

### Chat context
```plantuml
@startuml Chat Domain Map
!include Chat/Chat.Domain.puml
@enduml
```

### User context
```plantuml
@startuml User Domain Map
!include User/User.Domain.puml
@enduml
```

#### Constraints
- A *User* can interact with a chat only if it is a participant.
- A *User* can send messages within a chat only if it is a participant.
- A *User* can only delete messages in a chat if it's the original sender.
- *Chat* can't be deleted.
