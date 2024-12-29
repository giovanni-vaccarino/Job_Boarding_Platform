import { Internship } from '../internship/internship.ts';
import { Student } from '../student/student.ts';

export interface Match {
  hadInvite: boolean;
  internship: Internship;
  student: Student;
}
