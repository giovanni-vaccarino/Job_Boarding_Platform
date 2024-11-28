import { Page } from '../components/layout/Page.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { StudentJobDescription } from '../components/job-description/StudentJobDescription.tsx';
import { CompanyJobDescription } from '../components/job-description/CompanyJobDescription.tsx';
import {
  ApplicationStatus,
  JobDescriptionProps,
} from '../models/application/application.ts';

const testProps: JobDescriptionProps = {
  jobCategory: 'Technology',
  jobType: 'Full Time',
  location: 'London',
  postCreated: new Date('2022-08-01'),
  applicationDeadline: new Date('2022-08-12'),
  jobDescription: `We are searching for a software developer to build web applications for our company. In this role, you will design and create projects using Laravel framework and PHP, and assist the team in delivering high-quality web applications, services, and tools for our business.
    To ensure success as a Laravel developer you should be adept at utilizing Laravel's GUI and be able to design a PHP application from start to finish. A top-notch Laravel developer will be able to leverage their expertise and experience of the framework to independently produce complete solutions in a short turnaround time.`,
  skillsRequired: ['Python', 'Java'],
  status: ApplicationStatus.Ongoing,
};

export const JobDescription = (props: JobDescriptionProps) => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;
  props = testProps;

  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyJobDescription
          jobCategory={props.jobCategory}
          jobType={props.jobType}
          location={props.location}
          postCreated={props.postCreated}
          applicationDeadline={props.applicationDeadline}
          jobDescription={props.jobDescription}
          skillsRequired={props.skillsRequired}
          status={props.status}
        />
      ) : (
        <StudentJobDescription
          jobCategory={props.jobCategory}
          jobType={props.jobType}
          location={props.location}
          postCreated={props.postCreated}
          applicationDeadline={props.applicationDeadline}
          jobDescription={props.jobDescription}
          skillsRequired={props.skillsRequired}
          status={props.status}
        />
      )}
    </Page>
  );
};
