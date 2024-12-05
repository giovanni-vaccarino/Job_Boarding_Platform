import { Box, Stack, Typography } from '@mui/material';
import { JobListItem } from '../list-items/JobListItem.tsx';
import { TitleHeader } from '../page-headers/TitleHeader.tsx';

const importantJobList = [
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-15'),
  },
];

const jobList = [
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-14'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-13'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-12'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-11'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-10'),
  },
];

export const StudentMatches = () => {
  return (
    <>
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
    </>
  );
};
