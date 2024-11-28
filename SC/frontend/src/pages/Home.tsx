import { Page } from '../components/layout/Page.tsx';
import { HomePageHeader } from '../components/page-headers/HomePageHeader.tsx';
import { Box, Button, Stack, Typography } from '@mui/material';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { useAppSelector } from '../core/store';

const jobList = [
  {
    companyName: 'Google',
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

export const Home = () => {
  const searchMessage = useAppSelector((s) => s.global.searchMessage);
  const filteredJobs = jobList.filter(
    (job) =>
      job.companyName.toLowerCase().includes(searchMessage) ||
      job.jobTitle.toLowerCase().includes(searchMessage) ||
      job.location.toLowerCase().includes(searchMessage)
  );

  return (
    <Page>
      <HomePageHeader />

      <Typography sx={{ fontWeight: 'bold', fontSize: '1.7rem', mt: '1.5rem' }}>
        All Popular Job Listed
      </Typography>

      <Stack
        direction="column"
        spacing={4}
        mt={2}
        sx={{
          width: '100%',
          mt: '1rem',
          alignItems: 'center',
          pb: '4rem',
        }}
      >
        <Box sx={{ display: 'flex', gap: '2rem' }}>
          <Button
            variant="contained"
            sx={{
              backgroundColor: 'rgba(236, 241, 236, 1)',
              color: 'rgba(0, 0, 0, 0.6)',
              borderRadius: '7px',
              fontSize: '1rem',
              fontWeight: 'bold',
              padding: '0.5rem 0.8rem',
              minWidth: '1rem',
              '&:hover': {
                backgroundColor: 'rgba(220, 230, 220, 1)',
                boxShadow: 'none',
              },
            }}
            endIcon={
              <span style={{ color: 'rgba(0, 0, 0, 0.6)', fontSize: '12px' }}>
                ▼
              </span>
            }
          >
            Posted Date
          </Button>
        </Box>

        {filteredJobs.length > 0 ? (
          filteredJobs.map((job, index) => (
            <JobListItem
              key={index}
              companyName={job.companyName}
              jobTitle={job.jobTitle}
              location={job.location}
              datePosted={job.datePosted}
            />
          ))
        ) : (
          <Typography sx={{ fontStyle: 'italic' }}>
            No matching jobs found
          </Typography>
        )}
      </Stack>
    </Page>
  );
};
