import { CompanyMatches } from '../components/matches/CompanyMatches.tsx';
import { StudentMatches } from '../components/matches/StudentMatches.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { withAuth } from '../core/hoc/withAuth.tsx';
import { Page } from '../components/layout/Page.tsx';
import { useLoaderData } from 'react-router-dom';
import { Match } from '../models/match/match.ts';

//the test list contained also the intership invitation from other company
const testList = [
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-14'),
    hadInvite: true,
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-13'),
    hadInvite: true,
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-12'),
    hadInvite: true,
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-11'),
    hadInvite: false,
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-10'),
    hadInvite: false,
  },
];

export const Matches = withAuth(() => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  const matches = useLoaderData() as Match[];

  return (
    <Page>
      {profileType === TypeProfile.Student ? (
        <CompanyMatches matches={matches} />
      ) : (
        <StudentMatches matches={matches} />
      )}
    </Page>
  );
});
