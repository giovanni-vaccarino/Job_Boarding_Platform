import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { IInternshipApi } from './IInternshipApi.ts';
import { Internship } from '../../../models/internship/internship.ts';

@injectable()
export class InternshipApi extends ApiBase implements IInternshipApi {
  async getInternship(): Promise<Internship[]> {
    return await this.httpClient.get('/internship', {});
  }
  async getInternshipDetails(id: string): Promise<Internship> {
    return await this.httpClient.get('/internship/id', {});
  }
  async postApplyToInternship(): Promise<string> {
    return await this.httpClient.get('/internship', {});
  }
}
