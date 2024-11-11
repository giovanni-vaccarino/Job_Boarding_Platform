import { HttpClientBase } from './IHttpClient.ts';
import { injectable, inject } from 'inversify';
import { HttpClientFactory } from '../factories/HttpClientFactory.ts';
import { ServiceType } from '../../../ioc/service-type.ts';

@injectable()
export abstract class ApiBase {
  protected httpClient: HttpClientBase;
  constructor(
    @inject(ServiceType.HttpClientFactory)
    protected httpClientFactory: HttpClientFactory
  ) {
    this.httpClient = this.httpClientFactory.create();
  }
}
