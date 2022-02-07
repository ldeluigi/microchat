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
