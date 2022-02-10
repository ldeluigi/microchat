import { UserInfo } from "./UserInfo";

export interface User {
  id: string,
  name: string
}

export interface Chat {
  id: string,
  hasNewMessages: number,
  lastMessageTime: Date,
  user?: UserInfo
}

export interface ChatDto {
  creation: string,
  creator: string,
  id: string,
  partecipant: string
}

export interface ChatOfUser {
  id: string,
  creator: string,
  partecipant: string,
  creation: string,
  numberOfUnreadMessages?: number,
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