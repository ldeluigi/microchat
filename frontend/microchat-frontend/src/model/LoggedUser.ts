import { User } from "./Chat";

export interface LoggedUser {
  userId: string,
  accessToken: string,
  refreshToken: string,
  expirationDate: string
}

export interface AuthUserInfo {
  id: string,
  email: string,
  username: string
}

export function authToUser(userInfo: AuthUserInfo): User {
  return {id: userInfo.id, name: userInfo.username};
}