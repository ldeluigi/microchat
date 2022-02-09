# Chat Data Domain Model

## Chat subdomain class diagram
```plantuml
@startuml Chat SubDomain
!include chat-subdomain.puml
@enduml
```

### Domain Events

* **Chat created**: emitted when a chat is registered to the system.
* **Chat Deleted**: emitted when a chat is removed from the system.

## Private Chat context class diagram
```plantuml
@startuml PivateChat Context
!include private-chat-context.puml
@enduml
```
### Details

#### MessageText

**constraints**:

- $text < 4000.

## Chat constraints

- *Chat* can't be deleted.
- *PrivateChat* can have only two partecipants, a owner and a partecipant.
- A *User* can interact with a *Chat* only if it is a participant.
- A *User* can send messages within a *Chat* only if it is a participant.
- A *User* can only delete messages in a *Chat* if it's the original sender.