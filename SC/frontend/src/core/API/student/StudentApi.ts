import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { cvToSend, Student } from '../../../models/student/student.ts';
import { IStudentApi } from './IStudentApi.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';
import { Match } from '../../../models/match/match.ts';

@injectable()
export class StudentApi extends ApiBase implements IStudentApi {
  async getStudentInfo(studentId: string): Promise<Student> {
    console.log(studentId);
    return await this.httpClient.get(`student/${studentId}`, {});
  }
  async getApplications(studentId: string): Promise<ApplicationInfo[]> {
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
  async loadCvStudent(studentId: string, input: cvToSend): Promise<void> {
    const formData = new FormData();
    formData.append('File', input.file);

    return await this.httpClient.post(`student/cv/${studentId}`, formData);
  }
}
