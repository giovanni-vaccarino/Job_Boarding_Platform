import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { StudentsTable } from '../tables/StudentsTable.tsx';

const exampleData = [
  { name: 'John Doe', suggestedJob: 'Software Engineer' },
  { name: 'Jane Smith', suggestedJob: 'Data Scientist' },
  { name: 'Michael Brown', suggestedJob: 'Frontend Developer' },
  { name: 'Emily Davis', suggestedJob: 'Backend Developer' },
  { name: 'Chris Wilson', suggestedJob: 'Full Stack Engineer' },
];

export const CompanyMatches = () => {
  return (
    <>
      <TitleHeader title={'Matches'} />
      <Box sx={{ width: '90%', margin: '0 auto', marginTop: '1rem' }}>
        {exampleData.length > 0 ? (
          <StudentsTable students={exampleData} />
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
