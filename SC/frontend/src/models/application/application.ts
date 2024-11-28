export enum ApplicationStatus {
  OnlineAssessment,
  Ongoing,
  Ended,
}

export interface JobDescriptionInterface {
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: Date;
  applicationDeadline: Date;
  jobDescriptionMessage: string;
  skillsRequired: string[];
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
