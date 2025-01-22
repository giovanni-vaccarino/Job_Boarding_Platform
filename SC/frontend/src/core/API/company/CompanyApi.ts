import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { Company, Question } from '../../../models/company/company.ts';
import { ICompanyApi } from './ICompanyApi.ts';
import { AddInternshipDto } from '../../../models/internship/internship.ts';

@injectable()
export class CompanyApi extends ApiBase implements ICompanyApi {
  async getCompanyInfo(companyId: string): Promise<Company> {
    return await this.httpClient.get(`company/${companyId}`, {});
  }
  async updateCompanyInfo(companyId: string, input: Company): Promise<string> {
    return await this.httpClient.post<Company, string>(
      `company/${companyId}`,
      input
    );
  }
  async getCompanyQuestions(companyId: string): Promise<Question[]> {
    return await this.httpClient.get(`company/${companyId}/questions`, {});
  }

  async addInternship(
    companyId: string,
    newInternship: AddInternshipDto
  ): Promise<string> {
    return await this.httpClient.post<AddInternshipDto, string>(
      `company/${companyId}/internships`,
      newInternship
    );
  }
}
