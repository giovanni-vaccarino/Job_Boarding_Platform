import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { CompanyJobsTable } from '../components/tables/CompanyJobsTable.tsx';

const exampleData = [
  {
    title: 'Frontend Developer',
    applications: 5,
    jobType: 'Full Time',
    location: 'London',
  },
  {
    title: 'Backend Developer',
    applications: 3,
    jobType: 'Part Time',
    location: 'Berlin',
  },
  {
    title: 'Data Scientist',
    applications: 7,
    jobType: 'Remote',
    location: 'San Francisco',
  },
];

export const CompanyActivity = () => {
  return (
    <Page>
      <TitleHeader title={'Jobs List'} />
      <Box sx={{ display: 'flex', justifyContent: 'right', width: '80%' }}>
        <Typography
          sx={{
            color: 'primary.main',
            textDecoration: 'underline',
            fontStyle: 'italic',
            mt: '2rem',
          }}
        >
          Add New Job
        </Typography>
      </Box>

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
          <CompanyJobsTable jobs={exampleData} />
        ) : (
          <Typography sx={{ fontStyle: 'italic' }}>NO DATA</Typography>
        )}
      </Box>
    </Page>
  );
};
