import {
  HttpClientBase,
  HttpError,
  HttpMethod,
  IHttpClientConfig,
} from '../api-base/IHttpClient.ts';
import { appActions, appStore } from '../../../store';
import { LoginResponse } from '../../../../models/auth/login.ts';
import { HttpHeaders } from '../api-base/HttpHeader.ts';

export class RefreshAuthDecorator extends HttpClientBase {
  constructor(private httpClient: HttpClientBase) {
    super(httpClient.getBaseURL());
    this.retry = false;
  }
  private retry: boolean;

  private async sendRefreshTokenRequest() {
    const refreshToken = appStore.getState().auth.refreshToken;
    if (!refreshToken) {
      throw new HttpError(
        401,
        'NotAuthorized',
        'you do not have refresh token'
      );
    }
    const headers = new HttpHeaders();
    headers.set('refresh-token', refreshToken);

    const config: IHttpClientConfig = { headers };
    this.retry = true;
    const response = await this.post<{ refreshToken: string }, LoginResponse>(
      'authenticate/refresh',
      {
        refreshToken: refreshToken,
      },
      config
    );
    appStore.dispatch(
      appActions.auth.successLogin({
        refreshToken: refreshToken,
        accessToken: response.accessToken,
      })
    );
  }
  private async logout() {
    appStore.dispatch(appActions.auth.logout());
  }

  async request<TPayload, TResponse>(
    method: HttpMethod,
    url: string,
    config?: IHttpClientConfig,
    data?: TPayload
  ): Promise<TResponse> {
    try {
      return await this.httpClient.request<TPayload, TResponse>(
        method,
        url,
        config,
        data
      );
    } catch (cathcedError: unknown) {
      const error = cathcedError as HttpError;
      if (error.status === 401) {
        try {
          if (!this.retry) {
            await this.sendRefreshTokenRequest();
            return this.request(method, url, config, data);
          } else {
            await this.logout();
          }
        } catch (e) {
          console.error(e);
          await this.logout();
        }
      }

      throw error;
    }
  }
}
