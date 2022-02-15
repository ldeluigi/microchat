```plantuml
@startuml SendMessage
actor Sender
entity Auth
database AuthDB
entity "Chat (SignalR)" as Chat
database ChatDB
actor Receiver
Sender -> Auth : Login()
activate Auth
Auth -> AuthDB : ValidateAccount()
AuthDB --> Auth : ok
Auth -->  Sender : ValidToken
deactivate Auth
Sender -->  Chat : SendMessage()
activate Chat
Chat -> ChatDB : ValidateMessage()
activate ChatDB
ChatDB --> Chat : ok
Chat -> ChatDB : SaveMessage()
deactivate ChatDB
Chat --> Receiver : NewMessage()
Chat --> Sender : NewMessage()
deactivate Chat
@enduml

``````plantuml
@startuml NewUser
actor Customer
entity Auth
database AuthDB
entity RabbitMQ
entity User
database UserDB
Customer -> Auth : register()
activate Auth
Auth -> AuthDB : VerifyNewAccount()
activate AuthDB
AuthDB --> Auth : ok
Auth -> AuthDB : SaveAccount()
deactivate AuthDB
Auth --> RabbitMQ : AccountRegistered
activate RabbitMQ
RabbitMQ --> Auth : ack
Auth --> Customer : confirm
deactivate Auth
RabbitMQ -> User : AccountRegistered
activate User
User --> RabbitMQ : ack
deactivate RabbitMQ
User --> UserDB : VerifyNewUser()
activate UserDB
UserDB --> User : ok
User --> UserDB : SaveUser()
deactivate UserDB
deactivate User
@enduml
```

```plantuml
@startuml SendMessageMultipleReplicas
actor Sender
entity "SignalR Replica 1" as Chat
entity Redis
entity "SignalR Replica 2" as Chat2
actor Receiver
Sender --> Chat : Connect()
Receiver --> Chat2 : Connect()
Sender -->  Chat : SendMessage()
activate Chat
Chat -> Chat : ValidateMessage()
Chat --> Redis : NewMessage()
activate Redis
Redis --> Chat : ack
Redis -> Chat2 : NewMessage()
Chat2 --> Redis : ack
deactivate Redis
Chat2 --> Receiver : NewMessage()
Chat --> Sender : NewMessage()
deactivate Chat
@enduml