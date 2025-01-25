import {
  ApplicantInfo,
  ApplyToInternshipInput,
  Internship,
} from '../../../models/internship/internship.ts';
import {
  AllAnswersResponse,
  Question,
} from '../../../models/company/company.ts';
import {
  ApplicationInfo,
  ApplicationStatus,
} from '../../../models/application/application.ts';

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
  getApplicationsPerInternship: (
    internshipId: string,
    companyId: string
  ) => Promise<ApplicationInfo[]>;
  getApplicantInfo: (
    applicationId?: string,
    studentId?: string,
    companyId?: string
  ) => Promise<ApplicantInfo>;
  updateApplicationStatus: (
    applicationId: string,
    status: ApplicationStatus,
    companyId: string
  ) => Promise<string>;
}
