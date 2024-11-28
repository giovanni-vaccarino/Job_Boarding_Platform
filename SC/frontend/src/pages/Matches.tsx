import { CompanyMatches } from '../components/matches/CompanyMatches.tsx';
import { StudentMatches } from '../components/matches/StudentMatches.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { withAuth } from '../core/hoc/withAuth.tsx';
import { Page } from '../components/layout/Page.tsx';

export const Matches = withAuth(() => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyMatches />
      ) : (
        <StudentMatches />
      )}
    </Page>
  );
});
