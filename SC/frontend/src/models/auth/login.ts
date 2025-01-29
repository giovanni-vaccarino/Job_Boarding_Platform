import { TypeProfile } from './register.ts';

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  profileType: TypeProfile;
  profileId: number;
}

export interface LoginInput {
  email: string;
  password: string;
}

export interface SendVerificationEmailDto {
  email: string;
}

export interface UpdatePasswordDto {
  Token: string;
  Password: string;
}

export interface VerifyMailDto {
  VerificationToken: string;
}
