import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { Student } from '../../../models/student/student.ts';
import { IStudentApi } from './IStudentApi.ts';
import { Application } from '../../../models/application/application.ts';
import { Match } from '../../../models/match/match.ts';

@injectable()
export class StudentApi extends ApiBase implements IStudentApi {
  async getStudentInfo(studentId: string): Promise<Student> {
    console.log(studentId);
    return await this.httpClient.get(`student/${studentId}`, {});
  }
  async getApplications(studentId: string): Promise<Application[]> {
    return await this.httpClient.get(`student/${studentId}/applications`, {});
  }
  async getStudentMatches(studentId: string): Promise<Match[]> {
    return await this.httpClient.get(`student/${studentId}`);
  }
  async updateStudentInfo(studentId: string, input: Student): Promise<string> {
    return await this.httpClient.put<Student, string>(
      `student/${studentId}`,
      input
    );
  }
}
