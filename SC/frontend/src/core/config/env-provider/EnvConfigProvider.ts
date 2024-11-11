import { IConfigProvider } from './IConfigProvider.ts';

export class EnvConfigProvider implements IConfigProvider {
  for(): Map<string, string> {
    const envVariables = import.meta.env;
    const map = new Map<string, string>();
    for (const key in envVariables) {
      const value = envVariables[key];
      if (value) {
        map.set(key, value);
        if (key.includes('VITE_')) {
          const trimedKey = key.replace('VITE_', '');
          map.set(trimedKey, value);
        }
      }
    }
    return map;
  }
}
