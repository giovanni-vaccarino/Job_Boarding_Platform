import { IConfigService } from '../service/IConfigService.ts';
import { IConfigProvider } from '../env-provider/IConfigProvider.ts';

export interface IConfigServiceBuilder {
  addProvider(provider: IConfigProvider): IConfigServiceBuilder;
  build(): IConfigService;
}
