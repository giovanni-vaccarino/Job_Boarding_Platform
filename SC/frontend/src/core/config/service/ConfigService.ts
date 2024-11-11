import { IConfigService } from './IConfigService.ts';
import { injectable } from 'inversify';

@injectable()
export class ConfigService implements IConfigService {
  constructor(private configurations: Map<string, string>) {}

  get(key: string): string {
    const value = this.configurations.get(key);
    if (!value) {
      throw new Error('Configuration ' + key + ' not found');
    }

    return value;
  }
}
