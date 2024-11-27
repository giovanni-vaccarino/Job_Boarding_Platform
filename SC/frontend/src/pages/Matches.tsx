﻿import { Box, Stack, Typography } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { withAuth } from '../core/hoc/withAuth.tsx';

const importantJobList = [
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
];

const jobList = [
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: '2 weeks ago',
  },
];

export const Matches = withAuth(() => {
  return (
    <Page>
      {Object.keys(importantJobList).length > 0 && (
        <Box
          sx={{
            width: '100%',
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            mb: '3rem',
          }}
        >
          <TitleHeader title={'Invites'} />

          <Stack
            direction="column"
            spacing={2}
            sx={{
              width: '100%',
              mt: '3rem',
              alignItems: 'center',
            }}
          >
            {importantJobList.map((job, index) => (
              <JobListItem
                key={index}
                companyName={job.companyName}
                jobTitle={job.jobTitle}
                location={job.location}
                datePosted={job.datePosted}
                important={true}
              />
            ))}
          </Stack>
        </Box>
      )}

      <Box
        sx={{
          width: '100%',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          mb: '1rem',
        }}
      >
        <TitleHeader title="Jobs for you" />

        <Stack
          direction="column"
          spacing={2}
          sx={{
            width: '100%',
            mt: '3rem',
            alignItems: 'center',
          }}
        >
          {jobList.length > 0 ? (
            jobList.map((job, index) => (
              <JobListItem
                key={index}
                companyName={job.companyName}
                jobTitle={job.jobTitle}
                location={job.location}
                datePosted={job.datePosted}
              />
            ))
          ) : (
            <Typography sx={{ fontStyle: 'italic' }}>NO DATA</Typography>
          )}
        </Stack>
      </Box>
    </Page>
  );
});
