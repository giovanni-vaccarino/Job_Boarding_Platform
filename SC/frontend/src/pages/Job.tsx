import { Page } from '../components/layout/Page.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { StudentJobDescription } from '../components/job-description/StudentJobDescription.tsx';
import { CompanyJobDescription } from '../components/job-description/CompanyJobDescription.tsx';
import { JobDescriptionInterface } from '../models/application/application.ts';
import { useLoaderData } from 'react-router-dom';
import { Internship } from '../models/internship/internship.ts';

const mapInternshipDetailToJobDescription = (
  internship: Internship
): JobDescriptionInterface => {
  return {
    jobTitle: internship.title.toString(),
    jobCategory: internship.jobCategory?.toString(),
    jobType: internship.jobType?.toString(),
    location: internship.location,
    postCreated: internship.dateCreated.toString().split('T')[0],
    applicationDeadline: internship.applicationDeadline.toString(),
    jobDescriptionMessage: internship.description,
    skillsRequired: internship.requirements,
    jobId: internship.id,
  };
};

export const Job = () => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  const internship = useLoaderData() as Internship;

  //TODO the two are the same despite the apply button
  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyJobDescription
          jobDescription={mapInternshipDetailToJobDescription(internship)}
        />
      ) : (
        <StudentJobDescription
          jobDescription={mapInternshipDetailToJobDescription(internship)}
        />
      )}
    </Page>
  );
};
