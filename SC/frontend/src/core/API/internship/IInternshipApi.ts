import {
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import {
  AllAnswersResponse,
  Question,
} from '../../../models/company/company.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';

export interface IInternshipApi {
  getInternship: () => Promise<Internship[]>;
  getInternshipCompany: (id: string) => Promise<Internship[]>;
  getInternshipDetails: (id?: string) => Promise<Internship>;
  postApplyToInternship: (
    input: ApplyToInternshipInput
  ) => Promise<ApplicationInfo>;
  getInternshipQuestions: (id: string) => Promise<Question[]>;
  postAnswer: (
    idApplication: string,
    input: AllAnswersResponse,
    idStudent: string
  ) => Promise<string>;
  getApplicationsPerInternship: (id: string) => Promise<ApplicationInfo[]>;
}
