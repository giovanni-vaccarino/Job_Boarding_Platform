import { Page } from '../components/layout/Page.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { StudentJobDescription } from '../components/job-description/StudentJobDescription.tsx';
import { CompanyJobDescription } from '../components/job-description/CompanyJobDescription.tsx';
import { JobDescriptionInterface } from '../models/application/application.ts';
import {useLoaderData} from "react-router-dom";
import {Internship} from "../models/internship/internship.ts";

const testProps: JobDescriptionInterface = {
  jobCategory: 'Technology',
  jobType: 'Full Time',
  location: 'London',
  postCreated: new Date('2022-08-01'),
  applicationDeadline: new Date('2022-08-12'),
  jobDescriptionMessage: `We are searching for a software developer to build web applications for our company. In this role, you will design and create projects using Laravel framework and PHP, and assist the team in delivering high-quality web applications, services, and tools for our business.
    To ensure success as a Laravel developer you should be adept at utilizing Laravel's GUI and be able to design a PHP application from start to finish. A top-notch Laravel developer will be able to leverage their expertise and experience of the framework to independently produce complete solutions in a short turnaround time.`,
  skillsRequired: ['Python', 'Java'],
  jobId: 1,
};

const mapInternshipDetailToJobDescription = (internship: Internship): JobDescriptionInterface => {
  return {
    jobCategory: internship.jobCategory.toString(),
    jobType: internship.jobType.toString(),
    location: internship.location,
    postCreated: internship.dataCreated,
    applicationDeadline: internship.applicationDeadline,
    jobDescriptionMessage: internship.description,
    skillsRequired: internship.requirements,
    jobId: internship.id,
  };
};

export const Job = () => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;
  const props = testProps;

  //const internship = useLoaderData() as Internship;


  //TODO the two are the same despite the apply button
  return (
    <Page>
      {profileType === TypeProfile.Student ? (
        <CompanyJobDescription jobDescription={props} />
      ) : (
        <StudentJobDescription jobDescription={props} />
      )}
    </Page>
  );
};
