import { User } from "./Chat";

export interface UserInfo {
  id: string,
  name: string,
  surname: string,
  username: string
}

export function toUser(userInfo: UserInfo): User {
  return {id: userInfo.id, name: userInfo.username};
}

