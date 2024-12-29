import { StudentActivity } from '../components/activity/StudentActivity.tsx';
import { CompanyActivity } from '../components/activity/CompanyActivity.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { withAuth } from '../core/hoc/withAuth.tsx';
import { Page } from '../components/layout/Page.tsx';
import { useLoaderData } from 'react-router-dom';
import { Application } from '../models/application/application.ts';
import { Internship } from '../models/internship/internship.ts';

export const Activity = withAuth(() => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  //Depending on which profile the page should display the internship or the application
  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyActivity internship={useLoaderData() as Internship[]} />
      ) : (
        <StudentActivity applications={useLoaderData() as Application[]} />
      )}
    </Page>
  );
});
