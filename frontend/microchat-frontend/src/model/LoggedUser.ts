export interface LoggedUser {
  userId: string;
  accessToken: string;
  refreshToken: string;
}

export interface AuthUserInfo {
  id: string,
  email: string,
  username: string
}
