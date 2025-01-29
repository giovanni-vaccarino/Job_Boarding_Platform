import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase';
import {
  LoginInput,
  LoginResponse,
  SendVerificationEmailDto,
  UpdatePasswordDto,
  VerifyMailDto,
} from '../../../models/auth/login';
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
  async logout(): Promise<null> {
    return await this.httpClient.post<null, null>(
      '/authentication/logout',
      null
    );
  }
  async register(input: RegisterInput): Promise<RegisterResponse> {
    return await this.httpClient.post<RegisterInput, RegisterResponse>(
      '/authentication/register',
      input
    );
  }
  async sendResetPassword(dto: SendVerificationEmailDto): Promise<string> {
    return await this.httpClient.post<SendVerificationEmailDto, string>(
      '/authentication/send-reset-password',
      dto
    );
  }

  async resetPassword(dto: UpdatePasswordDto): Promise<string> {
    return await this.httpClient.post<UpdatePasswordDto, string>(
      '/authentication/reset-password',
      dto
    );
  }

  async sendVerificationMail(dto: SendVerificationEmailDto): Promise<string> {
    return await this.httpClient.post<SendVerificationEmailDto, string>(
      '/authentication/send-verification-email',
      dto
    );
  }

  async verifyMail(dto: VerifyMailDto): Promise<string> {
    return await this.httpClient.post<VerifyMailDto, string>(
      '/authentication/verify-email',
      dto
    );
  }
}
