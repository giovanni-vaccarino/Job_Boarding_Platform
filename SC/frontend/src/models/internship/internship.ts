export interface Internship {
  id: number;
  dataCreated: Date;
  title: string;
  duration: number;
  description: string;
  applicationDeadline: Date;
  location: string;
  jobCategory: JobCategory;
  jobType: JobType;
  requirements: string[];
  numApplications: number;
}

export enum JobCategory {
  Technology,
  Finance,
  Healthcare,
  Education,
  Marketing,
  Sales,
  HumanResources,
  Engineering,
  Legal,
  Operations,
}

export enum JobType {
  FullTime,
  PartTime,
}

export interface ApplyToInternshipInput {
  studentId?: string;
  internshipId?: string;
}
