import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { JobsTable } from '../components/tables/JobsTable.tsx';
import { Box } from '@mui/material';

export const Activity = () => {
  const exampleData = [
    {
      title: 'Software Engineer',
      company: 'TechCorp',
      state: 'Open',
      location: 'New York, NY',
      submissionDate: '2024-11-20',
    },
    {
      title: 'Data Scientist',
      company: 'DataWorks',
      state: 'Closed',
      location: 'San Francisco, CA',
      submissionDate: '2024-11-15',
    },
    {
      title: 'Product Manager',
      company: 'InnovateNow',
      state: 'Pending',
      location: 'Austin, TX',
      submissionDate: '2024-11-10',
    },
    {
      title: 'Software Engineer',
      company: 'TechCorp',
      state: 'Open',
      location: 'New York, NY',
      submissionDate: '2024-11-20',
    },
    {
      title: 'Data Scientist',
      company: 'DataWorks',
      state: 'Closed',
      location: 'San Francisco, CA',
      submissionDate: '2024-11-15',
    },
    {
      title: 'Product Manager',
      company: 'InnovateNow',
      state: 'Pending',
      location: 'Austin, TX',
      submissionDate: '2024-11-10',
    },
    {
      title: 'Software Engineer',
      company: 'TechCorp',
      state: 'Open',
      location: 'New York, NY',
      submissionDate: '2024-11-20',
    },
    {
      title: 'Data Scientist',
      company: 'DataWorks',
      state: 'Closed',
      location: 'San Francisco, CA',
      submissionDate: '2024-11-15',
    },
    {
      title: 'Product Manager',
      company: 'InnovateNow',
      state: 'Pending',
      location: 'Austin, TX',
      submissionDate: '2024-11-10',
    },
  ];

  return (
    <Page>
      <TitleHeader title={'My Jobs List'} />
      <Box sx={{ width: '90%', margin: '0 auto', marginTop: '1rem' }}>
        <JobsTable data={exampleData} />
      </Box>
    </Page>
  );
};
