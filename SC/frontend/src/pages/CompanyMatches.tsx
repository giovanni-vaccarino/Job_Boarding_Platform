import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { StudentsTable } from '../components/tables/StudentsTable.tsx';

const exampleData = [
  { name: 'John Doe', suggestedJob: 'Software Engineer' },
  { name: 'Jane Smith', suggestedJob: 'Data Scientist' },
  { name: 'Michael Brown', suggestedJob: 'Frontend Developer' },
  { name: 'Emily Davis', suggestedJob: 'Backend Developer' },
  { name: 'Chris Wilson', suggestedJob: 'Full Stack Engineer' },
];

export const CompanyMatches = () => {
  return (
    <Page>
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
    </Page>
  );
};
