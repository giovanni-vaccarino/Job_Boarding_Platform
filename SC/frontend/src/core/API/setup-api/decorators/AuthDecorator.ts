import {
  HttpClientBase,
  HttpMethod,
  IHttpClientConfig,
} from '../api-base/IHttpClient.ts';
import { appStore } from '../../../store';
import { HttpHeaders } from '../api-base/HttpHeader.ts';

export class AuthDecorator extends HttpClientBase {
  constructor(private httpClient: HttpClientBase) {
    super(httpClient.getBaseURL());
  }
  private addAuthHeader(config?: IHttpClientConfig): IHttpClientConfig {
    const accessToken = appStore.getState().auth.accessToken;
    if (!accessToken) return config || {};
    if (!config) {
      const headers = new HttpHeaders();
      headers.setAuthorization(`Bearer ${accessToken}`);
      return { headers };
    }

    if (!config.headers) {
      config.headers = new HttpHeaders();
      config.headers?.setAuthorization(`Bearer ${accessToken}`);
      return config;
    }
    config.headers.setAuthorization(`Bearer ${accessToken}`);

    return config;
  }
  request<TPayload, TResponse>(
    method: HttpMethod,
    url: string,
    config?: IHttpClientConfig,
    data?: TPayload
  ): Promise<TResponse> {
    config = this.addAuthHeader(config);
    return this.httpClient.request(method, url, config, data);
  }
}
