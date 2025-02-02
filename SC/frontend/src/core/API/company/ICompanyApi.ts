import {
  Company,
  Question,
  UpdateCompany,
} from '../../../models/company/company.ts';
import { AddInternshipDto } from '../../../models/internship/internship.ts';

export interface ICompanyApi {
  getCompanyInfo: (companyId: string) => Promise<Company>;
  updateCompanyInfo: (
    studentId: string,
    input: UpdateCompany
  ) => Promise<string>;
  getCompanyQuestions: (companyId: string) => Promise<Question[]>;
  addInternship: (
    companyId: string,
    newInternship: AddInternshipDto
  ) => Promise<string>;
}
