import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import {
  StudentsTable,
  StudentsTableHeader,
} from '../tables/StudentsTable.tsx';
import { Match } from '../../models/match/match.ts';

interface CompanyMatchesProps {
  matches: Match[];
}

const mapMatchToStudentsTableHeader = (match: Match): StudentsTableHeader => {
  return {
    name: match.student.name,
    suggestedJob: match.internship.title,
    studentId: match.student.id,
    matchId: match.id,
  };
};

//StudentsTable because it contains the suitable students for the company
export const CompanyMatches = (props: CompanyMatchesProps) => {
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
            sx={{
              fontStyle: 'italic',
              color: 'black',
              fontSize: '1.2rem',
              textAlign: 'center',
              mt: '2rem',
            }}
          >
            There are not available matches yet, update your skills to exploit
            the recommendation system
          </Typography>
        )}
      </Box>
    </>
  );
};
