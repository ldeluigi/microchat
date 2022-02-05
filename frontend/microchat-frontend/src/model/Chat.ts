export interface User {
  id: string,
  name: string
}

export interface Chat {
  id: string,
  hasNewMessages: number,
  user?: User
}

export function UserLeftChat(chat: Chat) {
  return chat.user ? true : false;
}