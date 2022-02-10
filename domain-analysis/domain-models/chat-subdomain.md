# Chat Domain Model

## Private Chat context class diagram
```plantuml
@startuml PrivateChat Context

!include meta/domain-analysis.metamodel.iuml

$aggregate(PrivateChat) {
    $aggregate_root(PrivateChat) {
        + id: Guid
        + creationTime: Timestamp
        + creator: Guid
        + partecipant: Guid
    }
}

$aggregate(User) {
    $aggregate_root(User) {
        + id: Guid
    }
}


$aggregate(PrivateMessage) {
    $aggregate_root(PrivateMessage) {
        + id: Guid
        + chatId: Guid
        + sendTime: Timestamp
        + lastEditTime: Option[Timestamp]
        + senderId: Option[Guid]
        + viewed: Bool
        + messageText : MessageText
        + editText(newText: MessageText, Timestamp)
        + setViewed()
    }
    
    $value(MessageText) {
        + text: String
    }

    MessageText --o PrivateMessage
}

$service(PrivateChatLifecycleService) {
    + createPrivateChat(creatorId: Guid, participantId: Guid): PrivateChat
    + deletePrivateChat(id: Guid): PrivateChat
}

User "1" <.. "0..*" PrivateChat: creator
User "1" <.. "0..*" PrivateMessage: sender
PrivateChat "1" <.. "0..*" PrivateMessage: chat
User "1" <... "0..*" PrivateChat: partecipant
@enduml
```
### Details

#### MessageText

**constraints**:

- $text.length < 4000$.

## Chat constraints

- *Chat* can't be deleted.
- *PrivateChat* can have only two partecipants, a owner and a partecipant.
- A *User* can interact with a *Chat* only if it is a participant.
- A *User* can send messages within a *Chat* only if it is a participant.
- A *User* can only delete messages in a *Chat* if it's the original sender.

## Domain Events

* **Chat created**: emitted when a chat is registered to the system.
* **Chat Deleted**: emitted when a chat is removed from the system.
