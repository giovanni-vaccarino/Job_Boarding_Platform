import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase';
import { LoginInput, LoginResponse } from '../../../models/auth/login';
import { IAuthApi } from './IAuthApi';
import {
  RegisterInput,
  RegisterResponse,
} from '../../../models/auth/register.ts';

@injectable()
export class AuthApi extends ApiBase implements IAuthApi {
  async login(input: LoginInput): Promise<LoginResponse> {
    return await this.httpClient.post<LoginInput, LoginResponse>(
      '/authentication/login',
      input
    );
  }
  async logout(): Promise<null>{
    return await this.httpClient.post<null, null>('logout', null);
  }
  async register(input: RegisterInput): Promise<RegisterResponse> {
    return await this.httpClient.post<RegisterInput, RegisterResponse>(
      '/authentication/register',
      input
    );
  }
}
