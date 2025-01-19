import { Internship } from '../internship/internship.ts';

export enum ApplicationStatus {
  OnlineAssessment,
  Ongoing,
  Ended,
}

export interface JobDescriptionInterface {
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: string;
  applicationDeadline: string;
  jobDescriptionMessage: string;
  skillsRequired: string[];
  jobId: number;
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
export interface Application {
  id: number;
  submissionDate: Date;
  applicationStatus: ApplicationStatus;
  internship: Internship;
}
