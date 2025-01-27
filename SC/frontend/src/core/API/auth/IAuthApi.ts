import { LoginInput, LoginResponse, SendVerificationEmailDto, UpdatePasswordDto } from '../../../models/auth/login';
import {
  RegisterInput,
  RegisterResponse,
} from '../../../models/auth/register.ts';

export interface IAuthApi {
  login: (input: LoginInput) => Promise<LoginResponse>;
  logout: () => Promise<null>;
  register: (input: RegisterInput) => Promise<RegisterResponse>;
  sendResetPassword: (dto: SendVerificationEmailDto) => Promise<string>;
  resetPassword: (dto: UpdatePasswordDto) => Promise<string>;
}
