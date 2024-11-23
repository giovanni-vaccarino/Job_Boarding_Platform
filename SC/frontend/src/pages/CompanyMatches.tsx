import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box } from '@mui/material';
import { StudentsTable } from '../components/tables/StudentsTable.tsx';

export const CompanyMatches = () => {
  const exampleData = [
    { name: 'John Doe', suggestedJob: 'Frontend Developer, intern' },
    { name: 'Jane Smith', suggestedJob: 'Backend Developer, intern' },
    { name: 'Robert Brown', suggestedJob: 'Data Scientist, intern' },
    {
      name: 'Emily Johnson',
      suggestedJob: 'Machine Learning Engineer, intern',
    },
    { name: 'Michael Davis', suggestedJob: 'DevOps Engineer, intern' },
    { name: 'Sarah Wilson', suggestedJob: 'Product Manager, intern' },
    { name: 'David Miller', suggestedJob: 'Full Stack Developer, intern' },
  ];

  return (
    <Page>
      <TitleHeader title={'Matches'} />
      <Box sx={{ width: '90%', margin: '0 auto', marginTop: '1rem' }}>
        <StudentsTable students={exampleData} />
      </Box>
    </Page>
  );
};
