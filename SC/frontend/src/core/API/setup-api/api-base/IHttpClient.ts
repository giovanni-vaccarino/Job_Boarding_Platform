import { HttpHeaders } from './HttpHeader.ts';

export interface IHttpClientConfig {
  headers?: HttpHeaders;
  params?: unknown;
}
export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

export class HttpError extends Error {
  constructor(
    public readonly status: number,
    public readonly code: string,
    public readonly message: string
  ) {
    super(message);
  }
}

export abstract class HttpClientBase {
  constructor(protected readonly baseURL: string) {}
  getBaseURL(): string {
    return this.baseURL;
  }
  abstract request<TPayload, TResponse>(
    method: HttpMethod,
    url: string,
    config?: IHttpClientConfig,
    data?: TPayload
  ): Promise<TResponse>;

  put<TPayload, TResponse>(
    url: string,
    payload: TPayload,
    config?: IHttpClientConfig
  ): Promise<TResponse> {
    return this.request<TPayload, TResponse>('PUT', url, config, payload);
  }
  patch<TPayload, TResponse>(
    url: string,
    payload: TPayload,
    config?: IHttpClientConfig
  ): Promise<TResponse> {
    return this.request<TPayload, TResponse>('PATCH', url, config, payload);
  }
  post<TPayload, TResponse>(
    url: string,
    payload: TPayload,
    config?: IHttpClientConfig
  ): Promise<TResponse> {
    return this.request<TPayload, TResponse>('POST', url, config, payload);
  }
  get<TResponse>(url: string, config?: IHttpClientConfig): Promise<TResponse> {
    return this.request<undefined, TResponse>('GET', url, config);
  }
  delete<TResponse>(
    url: string,
    config?: IHttpClientConfig
  ): Promise<TResponse> {
    return this.request<undefined, TResponse>('DELETE', url, config);
  }
}
