import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { Match } from '../../../models/match/match.ts';
import { IMatchApi } from './IMatchApi.ts';
import { ApplicantDetailsProps } from '../../../models/student/student.ts';

@injectable()
export class MatchApi extends ApiBase implements IMatchApi {
  async getMatchesStudent(studentId: string): Promise<Match[]> {
    return await this.httpClient.get(`student/${studentId}`, {});
  }
  async getMatchesCompany(companyId: string): Promise<Match[]> {
    return await this.httpClient.get(`company/${companyId}`, {});
  }
  async getApplicantDetails(
    studentId: string
  ): Promise<ApplicantDetailsProps> {}
{
  //TODO In the backend
  return await.this.httpClient.get('')
}
}
