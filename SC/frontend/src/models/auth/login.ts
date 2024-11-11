export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

export interface LoginInput {
  username: string;
  password: string;
}
