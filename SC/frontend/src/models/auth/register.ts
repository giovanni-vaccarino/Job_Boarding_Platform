export interface RegisterResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RegisterInput {
  email: string;
  password: string;
  confirmPassword: string;
  profile: TypeProfile;
}

export enum TypeProfile{
  Company, Student
}
