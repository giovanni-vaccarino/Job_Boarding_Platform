import { Page } from '../components/layout/Page.tsx';
import {
  ApplicationInterface,
  ApplicationStatus,
} from '../models/application/application.ts';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import ArrowCircleRightRoundedIcon from '@mui/icons-material/ArrowCircleRightRounded';
import { AppRoutes } from '../router.tsx';
import { JobDescriptionCore } from '../components/job-description/JobDescriptionCore.tsx';
import { CreateFeedback } from '../components/job-description/CreateFeedback.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';

const testProps: ApplicationInterface = {
  jobCategory: 'Technology',
  jobType: 'Full Time',
  location: 'London',
  postCreated: new Date('2022-08-01'),
  applicationDeadline: new Date('2022-08-12'),
  jobDescriptionMessage: `We are searching for a software developer to build web applications for our company. In this role, you will design and create projects using Laravel framework and PHP, and assist the team in delivering high-quality web applications, services, and tools for our business.
    To ensure success as a Laravel developer you should be adept at utilizing Laravel's GUI and be able to design a PHP application from start to finish. A top-notch Laravel developer will be able to leverage their expertise and experience of the framework to independently produce complete solutions in a short turnaround time.`,
  skillsRequired: ['Python', 'Java'],
  status: ApplicationStatus.OnlineAssessment,
  feedbackSelectable: false,
};

export const Application = () => {
  const props = testProps;

  const navigate = useNavigateWrapper();

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
            Status: {ApplicationStatus[props.status]}
          </Typography>
          {props.status === ApplicationStatus.OnlineAssessment && (
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

        <JobDescriptionCore jobDescription={props} />

        {props.status === ApplicationStatus.Ongoing &&
          props.feedbackSelectable && (
            <>
              <CreateFeedback />
            </>
          )}
      </Box>
    </Page>
  );
};
