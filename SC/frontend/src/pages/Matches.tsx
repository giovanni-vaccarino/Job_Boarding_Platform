import { CompanyMatches } from '../components/matches/CompanyMatches.tsx';
import { StudentMatches } from '../components/matches/StudentMatches.tsx';
import { useAppSelector } from '../core/store';
import { TypeProfile } from '../models/auth/register.ts';
import { withAuth } from '../core/hoc/withAuth.tsx';
import { Page } from '../components/layout/Page.tsx';
import {useLoaderData} from "react-router-dom";
import {Internship} from "../models/internship/internship.ts";
import {Match} from "../models/match/match.ts";

export const Matches = withAuth(() => {
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  const matches = useLoaderData() as Match[];

  return (
    <Page>
      {profileType === TypeProfile.Company ? (
        <CompanyMatches matches={matches}/>
      ) : (
        <StudentMatches/>
      )}
    </Page>
  );
});
