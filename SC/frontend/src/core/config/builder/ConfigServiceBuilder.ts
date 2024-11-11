import { IConfigServiceBuilder } from './IConfigServiceBuilder.ts';
import { ConfigService } from '../service/ConfigService.ts';
import { IConfigService } from '../service/IConfigService.ts';
import { IConfigProvider } from '../env-provider/IConfigProvider.ts';

export class ConfigServiceBuilder implements IConfigServiceBuilder {
  private configurations: Map<string, string>;

  constructor() {
    this.configurations = new Map<string, string>();
  }

  addProvider(provider: IConfigProvider): IConfigServiceBuilder {
    const providerConfig = provider.for();
    for (const [key, value] of providerConfig.entries()) {
      this.configurations.set(key, value);
    }

    return this;
  }

  build(): IConfigService {
    return new ConfigService(this.configurations);
  }
}