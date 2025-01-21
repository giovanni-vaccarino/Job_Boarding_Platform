import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { IInternshipApi } from './IInternshipApi.ts';
import {
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';
import {
  AllAnswersResponse,
  Question,
} from '../../../models/company/company.ts';

@injectable()
export class InternshipApi extends ApiBase implements IInternshipApi {
  async getInternship(): Promise<Internship[]> {
    return await this.httpClient.get('/internship', {});
  }
  async getInternshipCompany(companyId: string): Promise<Internship[]> {
    return await this.httpClient.get(`/company/${companyId}/internships`);
  }
  async getInternshipDetails(id?: string): Promise<Internship> {
    return await this.httpClient.get(`/internship/${id}`, {});
  }
  async postApplyToInternship(
    input: ApplyToInternshipInput
  ): Promise<ApplicationInfo> {
    return await this.httpClient.post(
      `/internship/apply-internship/${input.studentId}?internshipId=${input.internshipId}`,
      {}
    );
  }
  async getInternshipQuestions(internshipId: string): Promise<Question[]> {
    return await this.httpClient.get(`/internship/${internshipId}/questions`);
  }
  async postAnswer(
    idApplication: string,
    input: AllAnswersResponse,
    idStudent: string
  ): Promise<string> {
    return await this.httpClient.post(
      `internship/applications/${idApplication}?studentId=${idStudent}`,
      input
    );
  }
}
