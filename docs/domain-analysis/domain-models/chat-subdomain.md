# Chat Data Domain Model

## Chat subdomain class diagram
```plantuml
@startuml Chat SubDomain
!include meta/domain-analysis.metamodel.iuml

$entity(Chat) {
    + id: Guid
    + creationTime: DateTime
    + creatorId: Guid
}

$aggregate(User) {
    $aggregate_root(User) {
        + id: Guid
        + lastSeenTimestamp: Timestamp
    }
}

User "1" <.. "0..*" Chat: creator

@enduml
```

### Domain Events

* **Chat created**: emitted when a chat is registered to the system.
* **Chat Deleted**: emitted when a chat is removed from the system.

## Private Chat context class diagram
```plantuml
@startuml PrivateChat Context

!include meta/domain-analysis.metamodel.iuml

$aggregate(PrivateChat) {
    $aggregate_root(PrivateChat) {
        + id: Guid
        + creationTime: DateTime
        + creatorId: Guid
        + partecipantId: Guid
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
        + sendTime: DateTime
        + lastEditTime: Option[DateTime]
        + senderId: Option[Guid]
        + viewed: Bool
        + editText(newText: String, DateTime time): void
        + setViewed(): Void
    }
    
    $value(MessageText) {
        + text: String
    }

    MessageText --o PrivateMessage
}

$service(ChatService) {
    + createChat(participants: List[Guid]): Chat
    + getLastUsedChats(userId: Guid): List[Chat]
    + getChat(userId: Guid, chatId: Guid) : Chat
    + sendMessage(chatId: Guid, text: String, userId: Guid): Message
    + editMessage(messageId: Guid, text: String, userId: Guid): Message
    + deleteMessage(messageId: Guid, userId: Guid): Message
    + getLastSentMessages(userId: Guid): List[Message]
}

User "1" <... "0..*" PrivateChat: creator
User "1" <... "0..*" PrivateMessage: sender
PrivateMessage "0..*" -left-* "1" PrivateChat: isSentOn
User "1" <... "0..*" PrivateChat: partecipant
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