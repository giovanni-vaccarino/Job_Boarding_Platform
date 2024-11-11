export interface RegisterResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RegisterInput {
  username: string;
  password: string;
  profile: string;
}
