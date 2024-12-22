import { Internship } from '../../../models/internship/internship.ts';

export interface IInternshipApi {
  getInternship: () => Promise<Internship[]>;
  getInternshipDetails: (id: string) => Promise<Internship>;
  postApplyToInternship: () => Promise<string>;
}
