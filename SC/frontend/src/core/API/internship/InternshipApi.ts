import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { IInternshipApi } from './IInternshipApi.ts';

@injectable()
export class InternshipApi extends ApiBase implements IInternshipApi {
  async getJobs(): Promise<string> {
    return await this.httpClient.get('/internship', {});
  }
}
