export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  profileType?: string;
  profileId: number;
}

export interface LoginInput {
  email: string;
  password: string;
}
