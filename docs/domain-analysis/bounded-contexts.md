# Context Map


```plantuml
@startuml Domain
!include meta/context-map.metamodel.iuml

$subdomain "User Subdomain" {
    $context "User Context" as user
    $context "Auth Context" as auth

    $conformist(user, auth)
}

$subdomain "Chat Subdomain" {
    $context "Chat Context" as chat

    $conformist(chat, auth)
}

@enduml
```

Microchat has two subdomains:
* **User**: subdomain contains the cases for which the user is univocally involved.
* **Chat**: subdomain that includes the relationships between user and chat.
