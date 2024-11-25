import { Box } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { JobListItem } from '../components/list-items/JobListItem.tsx';

export const Home = () => {
  return (
    <Page>
      <Box
        sx={{
          width: '100%',
          display: 'flex',
          overflow: 'hidden',
          justifyContent: 'center',
          alignItems: 'center',
        }}
      >
        <JobListItem
          companyName={'Amazon'}
          jobTitle={'Software Engineer'}
          location={'Chicago'}
          datePosted={'2 weeks ago'}
        />
      </Box>
    </Page>
  );
};
