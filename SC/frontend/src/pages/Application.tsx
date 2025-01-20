import { Page } from '../components/layout/Page.tsx';
import {
  ApplicationInfo,
  ApplicationStatus,
  JobDescriptionInterface,
} from '../models/application/application.ts';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import ArrowCircleRightRoundedIcon from '@mui/icons-material/ArrowCircleRightRounded';
import { AppRoutes } from '../router.tsx';
import { JobDescriptionCore } from '../components/job-description/JobDescriptionCore.tsx';
import { CreateFeedback } from '../components/job-description/CreateFeedback.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useLoaderData } from 'react-router-dom';

const mapApplicationToJobsTableHeader = (
  application: ApplicationInfo
): JobDescriptionInterface => {
  return {
    jobCategory: application.internship.title,
    jobType: application.internship.jobType.toString(),
    location: application.internship.location,
    postCreated: application.submissionDate.toString(),
    applicationDeadline: application.internship.applicationDeadline.toString(),
    jobDescriptionMessage: application.internship.description,
    skillsRequired: application.internship.requirements,
    jobId: application.id,
  };
};

export const Application = () => {
  const applicationToShow = useLoaderData() as ApplicationInfo;

  const navigate = useNavigateWrapper();

  console.log('Application available:' + applicationToShow.applicationStatus);
  return (
    <Page>
      <TitleHeader title={'Software Engineering, intern - Amazon'} />

      <Box
        sx={{
          width: '60%',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'left',
          alignItems: 'left',
          mb: '1rem',
          mt: '1.5rem',
        }}
      >
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            gap: '1rem',
            cursor: 'pointer',
            mb: '1rem',
          }}
        >
          <Typography
            color="primary"
            sx={{
              fontWeight: 'bold',
              fontSize: '1.5rem',
            }}
          >
            Status: {applicationToShow.applicationStatus}
          </Typography>
          {applicationToShow.applicationStatus ===
            ApplicationStatus.OnlineAssessment && (
            <ArrowCircleRightRoundedIcon
              color="primary"
              onClick={() => navigate(AppRoutes.OnlineAssessment)}
              sx={{
                fontSize: '1.5rem',
                color: 'primary',
              }}
            />
          )}
        </Box>
        <JobDescriptionCore
          jobDescription={mapApplicationToJobsTableHeader(applicationToShow)}
        />
        {applicationToShow.applicationStatus === ApplicationStatus.Ongoing && (
          <>
            <CreateFeedback />
          </>
        )}
      </Box>
    </Page>
  );
};
