# Domain Driven Design

## Knowledge Crunching
The process of knowledge crunching involves stakeholder and domain experts (the team itself took these roles) and creates (iteratively) a refined _Ubiquitous Language_, _Context Map_ and _Domain Model_ through the dialogue and the collection of *User stories* (not reported here) and Use Cases.

!!! note
    The following documentation regards the analysis of a simplified Problem Space in order to focus (for the purposes of this project) to the technology know-how of scalable, full-fledged microservices systems based on DDD patterns and effective distributed systems design principles.

### Use Cases
```plantuml
@startuml Use Case Diagram
!include UseCaseScenario.puml
@enduml
```
There is only one type of user for our service. A user owns an account and is able to manage personal information and login data for it. With an account it's possibile to search other users

## Ubiquitous Language

|Term | Definition| Usage | Other meanings |
|-----|-----------|-------|----------------|
|Message| Piece of information sent in a chat by a user relative to a specific moment. | As a noun or as a verb. To message means to send a message. |
|Send| To deliver a message in a chat, where someone can see it in real time or later | As a verb, relative to messages. | The act of requesting the service to deliver a message to its destination. |
|Chat| Entity that contains messages and other metadata. A chat can involve at least one user.| As a noun. As a verb it means to send and/or receive messages. | The container of related messages. The communication medium between users. |
|Account| A collection of personal information about a user that enables the chat related functionalities. | As a noun, related to creation, deletion, update, personal information, authentication and authorization features. |
|User| A person with an account in the service. |

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

#### User context
```plantuml
@startuml User Domain Map
!include User/User.Domain.User.puml
@enduml
```

#### Auth context
```plantuml
@startuml Auth Domain Map
!include User/User.Domain.Auth.puml
@enduml
```


