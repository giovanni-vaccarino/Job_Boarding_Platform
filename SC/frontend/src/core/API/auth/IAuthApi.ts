import {
  LoginInput,
  LoginResponse,
  SendVerificationEmailDto,
  UpdatePasswordDto,
  VerifyMailDto,
} from '../../../models/auth/login';
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
  sendVerificationMail: (dto: SendVerificationEmailDto) => Promise<string>;
  verifyMail: (dto: VerifyMailDto) => Promise<string>;
}