export interface RegisterResponse {
  accessToken: string;
  refreshToken: string;
  profileId: number;
}

export interface RegisterInput {
  email: string;
  password: string;
  confirmPassword: string;
  profile: TypeProfile;
}

export enum TypeProfile {
  Company,
  Student,
}
