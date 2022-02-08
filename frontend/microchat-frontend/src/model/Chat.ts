export interface User {
  id: string,
  name: string
}

export interface Chat {
  id: string,
  hasNewMessages: number,
  lastMessageTime: Date,
  user?: User
}

export interface ChatDto {
  creation: string,
  creator: string,
  id: string,
  partecipant: string
}

export interface ChatOfUser {
  id: string,
  creatorId: string,
  partecipantId: string,
  CreationTimestamp: string,
  NumberOfUnreadMessages?: number,
  lastMessageTime: string
}

export interface DetailedChat {
    id: string,
    creation: string,
    numberOfMessages: number
}

export function UserLeftChat(chat: Chat) {
  return chat.user ? false : true;
}