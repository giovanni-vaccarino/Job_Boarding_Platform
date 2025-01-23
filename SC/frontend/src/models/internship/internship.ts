import { QuestionType } from '../company/company.ts';

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
  numberOfApplications: number;
  companyId: string;
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

export enum SkillsType {
  FrontEnd,
  BackEnd,
  FullStack,
  Mobile,
  DevOps,
  DataScience,
  MachineLearning,
  ArtificialIntelligence,
  CyberSecurity,
  CloudComputing,
  BlockChain,
}
export interface ApplyToInternshipInput {
  studentId?: string;
  internshipId?: string;
}

export interface AddInternshipDto {
  JobDetails: AddJobDetailsDto;
  Questions: AddQuestionDto[];
  ExistingQuestions: number[];
}

export interface AddJobDetailsDto {
  Title: string;
  Duration: DurationType;
  Description: string;
  ApplicationDeadline: Date;
  Location: string;
  JobCategory?: JobCategory;
  JobType?: JobType;
  Requirements: string[];
}

export enum DurationType {
  TwoToThreeMonths = 1,
  ThreeToSixMonths = 2,
  SixToTwelveMonths = 3,
  MoreThanOneYear = 4,
}

export interface AddQuestionDto {
  Title: string;
  QuestionType: QuestionType;
  Options: string[];
}
