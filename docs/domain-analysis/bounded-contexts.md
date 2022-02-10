# Bounded contexts
Microchat has two subdomains:
* **User**: subdomain contains the use cases for which the user is involved and it's the main focus.
* **Chat**: subdomain that includes the relationships between user and chat, and all the use cases wher instanti messaging and chats are the main focus.

## User subdomain
_Supporting subdomain_

### Auth context
Responsible for keeping login data, managing authentication for the entire system.

### User context
Responsible for keeping personal information unrelated from authentication, like name or surname. Provides endpoints to search for users with keywords.

## Chat subdomain
_Core subdomain_
### Private Chat context
Manages personal communication between users, with chats and messages.

## Context Map

### Legend
```plantuml
@startuml Legend
!include meta/context-map.metamodel.iuml
$print_legend()
@enduml
```

### Microchat context map
```plantuml
@startuml Domain
!include meta/context-map.metamodel.iuml

$subdomain "User Subdomain" {
    $context "User Context" as user
    $context "Auth Context" as auth

    $common_interface(auth, Account Lifecycle, lifecycle)
    $conformist(user, lifecycle, $interface=true)
}

$subdomain "Chat Subdomain" {
    $context "PrivateChat Context" as chat

    $conformist(chat, lifecycle, $interface=true)
}

@enduml
```
