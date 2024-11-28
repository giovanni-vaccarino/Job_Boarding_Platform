import { StudentActivity } from '../components/activity/StudentActivity.tsx';
import { CompanyActivity } from '../components/activity/CompanyActivity.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { withAuth } from '../core/hoc/withAuth.tsx';
import { Page } from '../components/layout/Page.tsx';

export const Activity = withAuth(() => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyActivity />
      ) : (
        <StudentActivity />
      )}
    </Page>
  );
});
