import { inject, injectable } from 'inversify';
import { ServiceType } from '../../../ioc/service-type.ts';
import type { IConfigService } from '../../../config/service/IConfigService.ts';
import { HttpClientBase } from '../api-base/IHttpClient.ts';
import { AxiosHttpClient } from '../api-base/AxiosHttpClient.ts';
import { AuthDecorator } from '../decorators/AuthDecorator.ts';
import { RefreshAuthDecorator } from '../decorators/RefreshAuthDecorator.ts';

@injectable()
export class HttpClientFactory {
  constructor(
    @inject(ServiceType.Config)
    private configService: IConfigService
  ) {}
  create(): HttpClientBase {
    let client: HttpClientBase = new AxiosHttpClient(this.configService);

    client = new AuthDecorator(client);
    client = new RefreshAuthDecorator(client);

    return client;
  }
}
