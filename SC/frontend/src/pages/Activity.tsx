import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { JobsTable } from '../components/tables/JobsTable.tsx';
import { Box, Typography } from '@mui/material';

const exampleData = [];

export const Activity = () => {
  return (
    <Page>
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
          <Typography sx={{ fontStyle: 'italic' }}>NO DATA</Typography>
        )}
      </Box>
    </Page>
  );
};
