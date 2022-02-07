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

export function UserLeftChat(chat: Chat) {
  return chat.user ? false : true;
}