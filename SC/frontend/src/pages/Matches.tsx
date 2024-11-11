import { Box, Stack } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { TitleHeader } from '../components/page-title/TitleHeader.tsx';

export const Matches = () => {
  return (
    <Page>
      <Box
        sx={{
          width: '100%',
          display: 'flex',
          overflow: 'hidden',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          mb: '1rem',
        }}
      >
        <TitleHeader title={'Jobs for you'} />

        <Stack
          direction="column"
          spacing={2}
          sx={{
            width: '100%',
            mt: '3rem',
            alignItems: 'center',
          }}
        >
          <JobListItem
            companyName={'Amazon'}
            jobTitle={'Software Engineer'}
            location={'Chicago'}
            datePosted={'2 weeks ago'}
          />

          <JobListItem
            companyName={'Amazon'}
            jobTitle={'Software Engineer'}
            location={'Chicago'}
            datePosted={'2 weeks ago'}
          />

          <JobListItem
            companyName={'Amazon'}
            jobTitle={'Software Engineer'}
            location={'Chicago'}
            datePosted={'2 weeks ago'}
          />

          <JobListItem
            companyName={'Amazon'}
            jobTitle={'Software Engineer'}
            location={'Chicago'}
            datePosted={'2 weeks ago'}
          />

          <JobListItem
            companyName={'Amazon'}
            jobTitle={'Software Engineer'}
            location={'Chicago'}
            datePosted={'2 weeks ago'}
          />
        </Stack>
      </Box>
    </Page>
  );
};
