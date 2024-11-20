import { Page } from '../components/layout/Page.tsx';
import { HomePageHeader } from '../components/page-headers/HomePageHeader.tsx';
import { Stack, Typography } from '@mui/material';
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

      <Typography sx={{ fontWeight: 'bold', fontSize: '2rem', mt: '2rem' }}>
        All Popular Job Listed
      </Typography>

      <Stack
        direction="column"
        spacing={2}
        mt={2}
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
      </Stack>
    </Page>
  );
};
