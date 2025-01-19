import {
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import { Application } from '../../../models/application/application.ts';

export interface IInternshipApi {
  getInternship: () => Promise<Internship[]>;
  getInternshipCompany: (id: string) => Promise<Internship[]>;
  getInternshipDetails: (id?: string) => Promise<Internship>;
  postApplyToInternship: (
    input: ApplyToInternshipInput
  ) => Promise<Application>;
}
