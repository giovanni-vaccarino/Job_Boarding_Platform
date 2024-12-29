import { Company } from '../../../models/company/company.ts';

export interface ICompanyApi {
  getCompanyInfo: (companyId: string) => Promise<Company>;
  updateCompanyInfo: (studentId: string, input: Company) => Promise<string>;
}
