interface User {
  id: string,
  name: string
}

export interface Chat {
  id: string,
  hasNewMessages: number,
  user?: User
}