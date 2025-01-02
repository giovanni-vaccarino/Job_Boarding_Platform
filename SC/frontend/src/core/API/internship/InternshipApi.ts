import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { IInternshipApi } from './IInternshipApi.ts';
import {
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import { Application } from '../../../models/application/application.ts';

@injectable()
export class InternshipApi extends ApiBase implements IInternshipApi {
  async getInternship(): Promise<Internship[]> {
    return await this.httpClient.get('/internship', {});
  }
  async getInternshipCompany(companyId: string): Promise<Internship[]> {
    return await this.httpClient.get(`/company/${companyId}/internships`);
  }
  async getInternshipDetails(id: string): Promise<Internship> {
    return await this.httpClient.get(`/internship/${id}`, {});
  }
  async postApplyToInternship(
    input: ApplyToInternshipInput
  ): Promise<Application> {
    return await this.httpClient.get(
      `/internship/apply-internship/${input.studentId}`,
      {}
    );
  }
}
