import { Internship } from '../internship/internship.ts';
import { Student } from '../student/student.ts';

export interface Match {
  id: string;
  hasInvite: boolean;
  internship: Internship;
  student: Student;
}
