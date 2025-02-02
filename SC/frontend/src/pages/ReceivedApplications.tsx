import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { ReceivedApplicationTable } from '../components/tables/ReceivedApplicationTable.tsx';
import { useLoaderData } from 'react-router-dom';
import { ApplicationInfo } from '../models/application/application.ts';

// Example data for the table
const exampleData = [
  {
    name: 'Mario Rossi',
    state: 'Ongoing',
    submissionDate: '13/06/2022',
  },
  {
    name: 'Luigi Bianchi',
    state: 'Under Review',
    submissionDate: '10/06/2022',
  },
  {
    name: 'Anna Verdi',
    state: 'Completed',
    submissionDate: '08/06/2022',
  },
];

export const ReceivedApplication = () => {
  const applications = useLoaderData() as ApplicationInfo[];

  console.log(applications);
  return (
    <Page>
      <TitleHeader title={'Received Applications'} />
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
          <ReceivedApplicationTable applications={applications} />
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
            There is no available applications yet
          </Typography>
        )}
      </Box>
    </Page>
  );
};
