import { Match } from '../../../models/match/match.ts';
import { ApplicantDetailsProps } from '../../../models/student/student.ts';

export interface IMatchApi {
  getMatchesStudent: (studentId: string) => Promise<Match[]>;
  getMatchesCompany: (companyId: string) => Promise<Match[]>;
  getApplicantDetails: (
    applicationId: string,
    studentId: string
  ) => Promise<ApplicantDetailsProps>;
  postInviteStudent: (matchId: string, companyId: string) => Promise<string>;
}
