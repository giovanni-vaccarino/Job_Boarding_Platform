import { inject, injectable } from 'inversify';
import type { IConfigService } from '../../../config/service/IConfigService.ts';

import {
  HttpClientBase,
  HttpError,
  HttpMethod,
  IHttpClientConfig,
} from './IHttpClient.ts';
import axios, {
  AxiosError,
  AxiosInstance,
  AxiosRequestConfig,
  RawAxiosRequestHeaders,
} from 'axios';
import { ServiceType } from '../../../ioc/service-type.ts';

@injectable()
export class AxiosHttpClient extends HttpClientBase {
  private client: AxiosInstance;

  constructor(
    @inject(ServiceType.Config)
    configService: IConfigService
  ) {
    super(configService.get('API_URL'));

    this.client = this.createInstance();
  }

  async request<TPayload, TResponse>(
    method: HttpMethod,
    url: string,
    config?: IHttpClientConfig | undefined,
    data?: TPayload | undefined
  ): Promise<TResponse> {
    const axiosConfig: AxiosRequestConfig = this.createAxiosConfig(config);
    axiosConfig.method = method;
    axiosConfig.data = data;
    axiosConfig.url = url;
    try {
      const response = await this.client.request<TResponse>(axiosConfig);
      return response.data;
    } catch (error) {
      const err = (await error) as AxiosError;
      if (err.code === 'ECONNABORTED') {
        return 'REQUEST ABORTED' as unknown as TResponse;
      }
      //const response = err.response as AxiosResponse<{ message: string }>;

      const errorMessage =
        typeof err?.response?.data === 'string'
          ? err.response.data.split('.')[1]?.trim()
          : err.message;

      throw new HttpError(
        err.response?.status || -1,
        err.code || 'UNKNOWN',
        errorMessage
      );
    }
  }

  private createInstance() {
    return axios.create({
      baseURL: this.baseURL,
    });
  }

  private createAxiosConfig(config?: IHttpClientConfig): AxiosRequestConfig {
    const headers: RawAxiosRequestHeaders = {
      ...config?.headers?.export(),
    };

    return {
      headers,
      params: config?.params,
    };
  }
}
