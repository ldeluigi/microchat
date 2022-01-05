interface User {
  id: string,
  name: string
}

export interface Chat {
  id: string,
  user?: User
}