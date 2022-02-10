const local_index = {"config":{"indexing":"full","lang":["en"],"min_search_length":3,"prebuild_index":false,"separator":"[\\s\\-]+"},"docs":[{"location":"index.html","text":"Introduction This autogenerated website stores all the relevant artifacts produced to document the process of designing the system following a Domain Driven approach as well as the final architecture of the solution. Project Proposal Members Thomas Angelini: thomas.angelini@studio.unibo.it Luca Deluigi: luca.deluigi3@studio.unibo.it Simone Magnani: simone.magnani4@studio.unibo.it Scenario The group wants to develop a chat. A system user could search for another user and start to communicate with him creating a chat with him. The goal is to let users communicate with each other sending messages in chats. Course requirements The system will be designed with a microservices approach. After an initial knowledge crunching session will be selected what microservices will be developed. In order to isolate microservice, the team will use Docker to put in container the microservices; but they needs to comunicate each other about what happened in each of them, so the communication will be implemented with an event bus. The team will have a tough challenge when choosing which of the CAP theorem they want to choose in the system.","title":"Introduction"},{"location":"index.html#introduction","text":"This autogenerated website stores all the relevant artifacts produced to document the process of designing the system following a Domain Driven approach as well as the final architecture of the solution.","title":"Introduction"},{"location":"index.html#project-proposal","text":"","title":"Project Proposal"},{"location":"index.html#members","text":"Thomas Angelini: thomas.angelini@studio.unibo.it Luca Deluigi: luca.deluigi3@studio.unibo.it Simone Magnani: simone.magnani4@studio.unibo.it","title":"Members"},{"location":"index.html#scenario","text":"The group wants to develop a chat. A system user could search for another user and start to communicate with him creating a chat with him. The goal is to let users communicate with each other sending messages in chats.","title":"Scenario"},{"location":"index.html#course-requirements","text":"The system will be designed with a microservices approach. After an initial knowledge crunching session will be selected what microservices will be developed. In order to isolate microservice, the team will use Docker to put in container the microservices; but they needs to comunicate each other about what happened in each of them, so the communication will be implemented with an event bus. The team will have a tough challenge when choosing which of the CAP theorem they want to choose in the system.","title":"Course requirements"},{"location":"SUMMARY.html","text":"Introduction Domain Exploration Domain Analysis","title":"SUMMARY"},{"location":"domain-analysis/SUMMARY.html","text":"Bounded Contexts Domain Models","title":"SUMMARY"},{"location":"domain-analysis/bounded-contexts.html","text":"Context Map Microchat has two subdomains: * User : subdomain contains the cases for which the user is univocally involved. * Chat : subdomain that includes the relationships between user and chat.","title":"Bounded Contexts"},{"location":"domain-analysis/bounded-contexts.html#context-map","text":"Microchat has two subdomains: * User : subdomain contains the cases for which the user is univocally involved. * Chat : subdomain that includes the relationships between user and chat.","title":"Context Map"},{"location":"domain-analysis/domain-models/chat-subdomain.html","text":"Chat Domain Model Private Chat context class diagram Details MessageText constraints : \\(text.length < 4000\\) . Chat constraints Chat can't be deleted. PrivateChat can have only two partecipants, a owner and a partecipant. A User can interact with a Chat only if it is a participant. A User can send messages within a Chat only if it is a participant. A User can only delete messages in a Chat if it's the original sender. Domain Events Chat created : emitted when a chat is registered to the system. Chat Deleted : emitted when a chat is removed from the system.","title":"Chat Domain Model"},{"location":"domain-analysis/domain-models/chat-subdomain.html#chat-domain-model","text":"","title":"Chat Domain Model"},{"location":"domain-analysis/domain-models/chat-subdomain.html#private-chat-context-class-diagram","text":"","title":"Private Chat context class diagram"},{"location":"domain-analysis/domain-models/chat-subdomain.html#details","text":"","title":"Details"},{"location":"domain-analysis/domain-models/chat-subdomain.html#messagetext","text":"constraints : \\(text.length < 4000\\) .","title":"MessageText"},{"location":"domain-analysis/domain-models/chat-subdomain.html#chat-constraints","text":"Chat can't be deleted. PrivateChat can have only two partecipants, a owner and a partecipant. A User can interact with a Chat only if it is a participant. A User can send messages within a Chat only if it is a participant. A User can only delete messages in a Chat if it's the original sender.","title":"Chat constraints"},{"location":"domain-analysis/domain-models/chat-subdomain.html#domain-events","text":"Chat created : emitted when a chat is registered to the system. Chat Deleted : emitted when a chat is removed from the system.","title":"Domain Events"},{"location":"domain-analysis/domain-models/legend.html","text":"","title":"Legend"},{"location":"domain-analysis/domain-models/user-subdomain.html","text":"User Domain Model User context Details Name and Surname constraints : $value must have almost 4 letters. $value can't have more than 100 letters. Username constraints : $value must have almost 4 letters. value matches ^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$ Auth context Details Username constraints : $value must have almost 4 letters. $value matches ^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$ Email constraints : $value must respect regex for email. Password Hash constraints : $password must not be empty. $password must have almost 8 characters. Domain Events Account created : emitted when an account is registered to the system. Account Unregistered : emitted when an account is removed from the system.","title":"User Domain Model"},{"location":"domain-analysis/domain-models/user-subdomain.html#user-domain-model","text":"","title":"User Domain Model"},{"location":"domain-analysis/domain-models/user-subdomain.html#user-context","text":"","title":"User context"},{"location":"domain-analysis/domain-models/user-subdomain.html#details","text":"","title":"Details"},{"location":"domain-analysis/domain-models/user-subdomain.html#name-and-surname","text":"constraints : $value must have almost 4 letters. $value can't have more than 100 letters.","title":"Name and Surname"},{"location":"domain-analysis/domain-models/user-subdomain.html#username","text":"constraints : $value must have almost 4 letters. value matches ^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$","title":"Username"},{"location":"domain-analysis/domain-models/user-subdomain.html#auth-context","text":"","title":"Auth context"},{"location":"domain-analysis/domain-models/user-subdomain.html#details_1","text":"","title":"Details"},{"location":"domain-analysis/domain-models/user-subdomain.html#username_1","text":"constraints : $value must have almost 4 letters. $value matches ^[A-Za-z][A-Za-z0-9]*(_[A-Za-z0-9]{2,}){0,3}$","title":"Username"},{"location":"domain-analysis/domain-models/user-subdomain.html#email","text":"constraints : $value must respect regex for email.","title":"Email"},{"location":"domain-analysis/domain-models/user-subdomain.html#password-hash","text":"constraints : $password must not be empty. $password must have almost 8 characters.","title":"Password Hash"},{"location":"domain-analysis/domain-models/user-subdomain.html#domain-events","text":"Account created : emitted when an account is registered to the system. Account Unregistered : emitted when an account is removed from the system.","title":"Domain Events"},{"location":"domain-exploration/SUMMARY.html","text":"Knowledge Crunching process Use Cases Ubiquitous Language","title":"SUMMARY"},{"location":"domain-exploration/knowledge-crunching.html","text":"Knowledge Crunching The process of knowledge crunching involves stakeholder and domain experts (the team itself took these roles) and creates (iteratively) a refined Ubiquitous Language , Context Map and Domain Model through the dialogue and the collection of User stories (not reported here) and Use Cases. Note The following documentation regards the analysis of a simplified Problem Space in order to focus (for the purposes of this project) to the technology know-how of scalable, full-fledged microservices systems based on DDD patterns and effective distributed systems design principles.","title":"Knowledge Crunching process"},{"location":"domain-exploration/knowledge-crunching.html#knowledge-crunching","text":"The process of knowledge crunching involves stakeholder and domain experts (the team itself took these roles) and creates (iteratively) a refined Ubiquitous Language , Context Map and Domain Model through the dialogue and the collection of User stories (not reported here) and Use Cases. Note The following documentation regards the analysis of a simplified Problem Space in order to focus (for the purposes of this project) to the technology know-how of scalable, full-fledged microservices systems based on DDD patterns and effective distributed systems design principles.","title":"Knowledge Crunching"},{"location":"domain-exploration/ubiquitous-language.html","text":"Ubiquitous Language Term Definition Usage Other meanings Message Piece of information sent in a chat by a user relative to a specific moment. As a noun or as a verb. To message means to send a message. Chat Entity that contains messages and other metadata. A chat can involve at least one user. As a noun. As a verb it means to send and/or receive messages. The container of related messages. The communication medium between users. Account A collection of personal information about a user that enables chat related functionalities. As a noun, related to creation, deletion, update, personal information, authentication and authorization features. User A person with an account in the service. As a noun, related to manage chat and personal information. Send To deliver a message in a chat, where some users can see it in real time or later. As a verb, relative to messages. The act of requesting the service to deliver a message to its destination. Search To search a user in the system, to start a converstion with him. As a verb, relative to users. The act of requesting the service to search a user. Private Chat Chat that involves only two users, a creator and a partecipant. Private Message Message sent in a private chat. It contains additional information than normal message.","title":"Ubiquitous Language"},{"location":"domain-exploration/ubiquitous-language.html#ubiquitous-language","text":"Term Definition Usage Other meanings Message Piece of information sent in a chat by a user relative to a specific moment. As a noun or as a verb. To message means to send a message. Chat Entity that contains messages and other metadata. A chat can involve at least one user. As a noun. As a verb it means to send and/or receive messages. The container of related messages. The communication medium between users. Account A collection of personal information about a user that enables chat related functionalities. As a noun, related to creation, deletion, update, personal information, authentication and authorization features. User A person with an account in the service. As a noun, related to manage chat and personal information. Send To deliver a message in a chat, where some users can see it in real time or later. As a verb, relative to messages. The act of requesting the service to deliver a message to its destination. Search To search a user in the system, to start a converstion with him. As a verb, relative to users. The act of requesting the service to search a user. Private Chat Chat that involves only two users, a creator and a partecipant. Private Message Message sent in a private chat. It contains additional information than normal message.","title":"Ubiquitous Language"},{"location":"domain-exploration/use-case.html","text":"Use Cases There is only one type of user for our service. A user owns an account and is able to manage personal information and login data for it. With an account, it's possible to search for other users and create a chat with them. In a chat it's possible to send messages to other users. Once a user has sent a message, he can edit or delete it. The system currently provides","title":"Use Cases"},{"location":"domain-exploration/use-case.html#use-cases","text":"There is only one type of user for our service. A user owns an account and is able to manage personal information and login data for it. With an account, it's possible to search for other users and create a chat with them. In a chat it's possible to send messages to other users. Once a user has sent a message, he can edit or delete it. The system currently provides","title":"Use Cases"}]}; var __search = { index: Promise.resolve(local_index) }