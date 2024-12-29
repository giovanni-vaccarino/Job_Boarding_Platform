import { Student } from '../../../models/student/student.ts';
import { Application } from '../../../models/application/application.ts';
import { Match } from '../../../models/match/match.ts';

export interface IStudentApi {
  getStudentInfo: (studentId: string) => Promise<Student>;
  getApplications: (studentId: string) => Promise<Application[]>;
  getStudentMatches: (studentId: string) => Promise<Match[]>;
  updateStudentInfo: (studentId: string, input: Student) => Promise<string>;
}
