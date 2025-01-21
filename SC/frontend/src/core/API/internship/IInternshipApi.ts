import {
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import { Question } from '../../../models/company/company.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';

export interface IInternshipApi {
  getInternship: () => Promise<Internship[]>;
  getInternshipCompany: (id: string) => Promise<Internship[]>;
  getInternshipDetails: (id?: string) => Promise<Internship>;
  postApplyToInternship: (
    input: ApplyToInternshipInput
  ) => Promise<ApplicationInfo>;
  getInternshipQuestions: (id: string) => Promise<Question[]>;
}
