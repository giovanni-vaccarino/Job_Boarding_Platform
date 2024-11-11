import { LoginInput, LoginResponse } from '../../../models/auth/login';
import {
  RegisterInput,
  RegisterResponse,
} from '../../../models/auth/register.ts';

export interface IAuthApi {
  login: (input: LoginInput) => Promise<LoginResponse>;

  register: (input: RegisterInput) => Promise<RegisterResponse>;
}
