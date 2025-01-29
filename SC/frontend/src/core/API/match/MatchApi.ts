import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { Match } from '../../../models/match/match.ts';
import { IMatchApi } from './IMatchApi.ts';
import { ApplicantDetailsProps } from '../../../models/student/student.ts';

@injectable()
export class MatchApi extends ApiBase implements IMatchApi {
  async getMatchesStudent(studentId: string): Promise<Match[]> {
    return await this.httpClient.get(`matches/student/${studentId}`, {});
  }
  async getMatchesCompany(companyId: string): Promise<Match[]> {
    return await this.httpClient.get(`matches/company/${companyId}`, {});
  }
  //TODO this should be added in match controller in backend
  async getApplicantDetails(
    applicationId: string,
    studentId: string
  ): Promise<ApplicantDetailsProps> {
    return await this.httpClient.get(
      `applications/applicantInfo/${applicationId}?studentId=${studentId}`,
      {}
    );
  }

  async postInviteStudent(matchId: string, companyId: string): Promise<string> {
    return await this.httpClient.patch(
      `matches/${matchId}?companyId=${companyId}`,
      companyId
    );
  }
}
