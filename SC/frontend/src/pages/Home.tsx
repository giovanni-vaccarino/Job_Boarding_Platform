import { Page } from '../components/layout/Page.tsx';
import { HomePageHeader } from '../components/page-headers/HomePageHeader.tsx';
import { Box, Button, Stack, Typography } from '@mui/material';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { useEffect } from 'react';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IInternshipApi } from '../core/API/internship/IInternshipApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';

export const Home = () => {
  const internshipApi = useService<IInternshipApi>(ServiceType.InternshipApi);

  useEffect(() => {
    const fetchJobs = async () => {
      const res = await internshipApi.getJobs();

      console.log(res);
    };

    fetchJobs();
  }, [internshipApi]);

  return (
    <Page>
      <HomePageHeader />

      <Typography sx={{ fontWeight: 'bold', fontSize: '1.7rem', mt: '2rem' }}>
        All Popular Job Listed
      </Typography>

      <Stack
        direction="column"
        spacing={2}
        mt={2}
        sx={{
          width: '100%',
          mt: '2rem',
          alignItems: 'center',
          pb: '4rem',
        }}
      >
        <Box sx={{ display: 'flex', gap: '2rem' }}>
          <Button
            variant="contained"
            sx={{
              backgroundColor: 'rgba(236, 241, 236, 1)', // Light greenish background
              color: 'rgba(0, 0, 0, 0.6)', // Grey text
              borderRadius: '7px', // Rounded corners
              fontSize: '1rem', // Font size
              fontWeight: 'bold',
              padding: '0.5rem 0.8rem', // Button padding
              minWidth: '1rem', // Minimum button size
              '&:hover': {
                backgroundColor: 'rgba(220, 230, 220, 1)', // Slightly darker on hover
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

          <Button
            variant="contained"
            sx={{
              backgroundColor: 'rgba(236, 241, 236, 1)', // Light greenish background
              color: 'rgba(0, 0, 0, 0.6)', // Grey text
              borderRadius: '7px', // Rounded corners
              fontSize: '1rem', // Font size
              fontWeight: 'bold',
              padding: '0.5rem 0.8rem', // Button padding
              minWidth: '1rem', // Minimum button size
              '&:hover': {
                backgroundColor: 'rgba(220, 230, 220, 1)', // Slightly darker on hover
                boxShadow: 'none',
              },
            }}
            endIcon={
              <span style={{ color: 'rgba(0, 0, 0, 0.6)', fontSize: '12px' }}>
                ▼
              </span>
            }
          >
            Location
          </Button>
        </Box>

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
    </Page>
  );
};
