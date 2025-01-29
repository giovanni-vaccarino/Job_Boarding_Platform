import { cvToSend, Student } from '../../../models/student/student.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';
import { Match } from '../../../models/match/match.ts';

export interface IStudentApi {
  getStudentInfo: (studentId: string) => Promise<Student>;
  getApplications: (studentId: string) => Promise<ApplicationInfo[]>;
  getStudentMatches: (studentId: string) => Promise<Match[]>;
  updateStudentInfo: (studentId: string, input: Student) => Promise<string>;
  loadCvStudent: (studentId: string, input: cvToSend) => Promise<void>;
}
