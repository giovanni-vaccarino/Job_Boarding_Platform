export interface RegisterResponse {
  accessToken: string;
  refreshToken: string;
  profileId: number;
}

export interface RegisterInput {
  email: string;
  password: string;
  confirmPassword: string;
  profileType: TypeProfile;
}

export enum TypeProfile {
  Student = 0,
  Company = 1,
}
