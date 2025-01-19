import { Feedback } from '../feedback/feedback.ts';

export interface Student {
  cf: string;
  cvPath: string;
  email: string;
  id: string;
  interests: string[];
  name: string;
  skills: string[];
}

export interface ApplicantDetailsProps {
  student: Student;
  feedback: Feedback[];
}
