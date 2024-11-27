import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { JobsTable } from '../tables/JobsTable.tsx';
import { Box, Typography } from '@mui/material';

const exampleData = [
  {
    title: 'Software Engineer',
    company: 'Tech Solutions Inc.',
    state: 'California',
    location: 'San Francisco',
    submissionDate: '2024-11-25',
  },
  {
    title: 'Data Analyst',
    company: 'Data Insights LLC',
    state: 'New York',
    location: 'New York City',
    submissionDate: '2024-11-20',
  },
  {
    title: 'Product Manager',
    company: 'Innovatech Corp.',
    state: 'Texas',
    location: 'Austin',
    submissionDate: '2024-11-15',
  },
];

export const StudentActivity = () => {
  return (
    <>
      <TitleHeader title={'My Jobs List'} />
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          width: '90%',
          margin: '0 auto',
          marginTop: '1rem',
        }}
      >
        {exampleData.length > 0 ? (
          <JobsTable jobs={exampleData} />
        ) : (
          <Typography sx={{ fontStyle: 'italic' }}>
            THERE IS NO JOB AVAILABLE
          </Typography>
        )}
      </Box>
    </>
  );
};
