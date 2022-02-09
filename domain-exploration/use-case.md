# Use Cases
```plantuml
@startuml Use Case Diagram

left to right direction
actor "User" as user

rectangle "Use Cases" {
    usecase "manage account" as manage_account
    usecase "search user" as search_user
    usecase "manage chat" as manage_chat
    usecase "manage message" as manage_message
    usecase "modify password" as modify_password
}
user --> manage_account
user --> search_user
user --> manage_chat
user --> manage_message
user --> modify_password
@enduml
```

There is only one type of user for our service. A user owns an account and is able to manage personal information and login data for it. With an account, it's possible to search for other users and create a chat with them. In a chat it's possible to send messages to other users. Once a user has sent a message, he can edit or delete it. The system currently provides 