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
!include Chat.Domain.puml
@enduml
```
#### Constraints
A *User* can interact with a chat only if it is a participant.
A *User* can only delete messages in a chat if it's the original sender.

*Chat* can't be deleted.