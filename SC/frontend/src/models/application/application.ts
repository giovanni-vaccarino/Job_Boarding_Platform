export enum ApplicationStatus {
  NotApplied,
  OnlineAssessment,
  Ongoing,
  Ended,
}

export interface JobDescriptionProps {
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: Date;
  applicationDeadline: Date;
  jobDescription: string;
  skillsRequired: string[];
  status?: ApplicationStatus;
  feedbackSelectable?: boolean;
}
