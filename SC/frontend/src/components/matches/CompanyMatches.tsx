import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { StudentsTable } from '../tables/StudentsTable.tsx';
import { Match } from '../../models/match/match.ts';

const exampleData = [
  { name: 'John Doe', suggestedJob: 'Software Engineer' },
  { name: 'Jane Smith', suggestedJob: 'Data Scientist' },
  { name: 'Michael Brown', suggestedJob: 'Frontend Developer' },
  { name: 'Emily Davis', suggestedJob: 'Backend Developer' },
  { name: 'Chris Wilson', suggestedJob: 'Full Stack Engineer' },
];

interface CompanyMatchesProps {
    matches: Match[];
}

export const CompanyMatches = ({ matches }: CompanyMatchesProps) => {
  return (
    <>
      <TitleHeader title={'Matches'} />
      <Box sx={{ width: '90%', margin: '0 auto', marginTop: '1rem' }}>
        {matches.length > 0 ? (
          <StudentsTable students={matches} />
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
