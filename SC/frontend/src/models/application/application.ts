import { Internship } from '../internship/internship.ts';
import { Student } from '../student/student.ts';
import { Feedback } from '../feedback/feedback.ts';

export interface UpdateStatusApplicationDto {
  status: ApplicationStatus;
}

export enum ApplicationStatus {
  Accepted,
  Rejected,
  Screening,
  OnlineAssessment,
  LastEvaluation,
}

export interface JobDescriptionInterface {
  jobTitle: string;
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: string;
  applicationDeadline: string;
  jobDescriptionMessage: string;
  skillsRequired: string[];
  jobId: number;
  feedbacks: Feedback[];
}

export interface JobDescriptionProps {
  jobDescription: JobDescriptionInterface;
}

export interface ApplicationInterface extends JobDescriptionInterface {
  status: ApplicationStatus;
  feedbackSelectable: boolean;
}

export interface ApplicationProps {
  applicationDescription: ApplicationInterface;
}

//TODO to adapt this interface to the above one
export interface ApplicationInfo {
  id: number;
  submissionDate: Date;
  applicationStatus: ApplicationStatus;
  internship: Internship;
  student: Student;
  companyName: string;
}
