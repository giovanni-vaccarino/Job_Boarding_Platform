import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import {
  StudentsTable,
  StudentsTableHeader,
} from '../tables/StudentsTable.tsx';
import { Match } from '../../models/match/match.ts';
import { useService } from '../../core/ioc/ioc-provider.tsx';
import { ICompanyApi } from '../../core/API/company/ICompanyApi.ts';
import { ServiceType } from '../../core/ioc/service-type.ts';
import { IMatchApi } from '../../core/API/match/IMatchApi.ts';


interface CompanyMatchesProps {
  matches: Match[];
}

const mapMatchToStudentsTableHeader = (match: Match): StudentsTableHeader => {
  return {
    name: match.student.name,
    suggestedJob: match.internship.title,
  };
};

//StudentsTable because it contains the suitable students for the company
export const CompanyMatches = (props: CompanyMatchesProps) => {

  console.log(props.matches)

  return (
    <>
      <TitleHeader title={'Matches'} />
      <Box sx={{ width: '90%', margin: '0 auto', marginTop: '1rem' }}>
        {props.matches.length > 0 ? (
          <StudentsTable
            students={props.matches.map(mapMatchToStudentsTableHeader)}
          />
        ) : (
          <Typography
            sx={{ fontStyle: 'italic', textAlign: 'center', mt: '3rem' }}
          >
            NO DATA
          </Typography>
        )}
      </Box>
    </>
  );
};
