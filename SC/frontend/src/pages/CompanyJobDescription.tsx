import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../router.tsx';
import ArrowCircleRightRoundedIcon from '@mui/icons-material/ArrowCircleRightRounded';

export interface CompanyJobDescriptionProps {
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: Date;
  applicationDeadline: Date;
  jobDescription: string;
  skillsRequired: string[];
}

//TODO ADD PROPS WHEN CALLING THE FUNCTION
export const CompanyJobDescription = () => {
  const navigate = useNavigateWrapper();

  const testProps: CompanyJobDescriptionProps = {
    jobCategory: 'Technology',
    jobType: 'Full Time',
    location: 'London',
    postCreated: new Date('2022-08-01'),
    applicationDeadline: new Date('2022-08-12'),
    jobDescription: `We are searching for a software developer to build web applications for our company. In this role, you will design and create projects using Laravel framework and PHP, and assist the team in delivering high-quality web applications, services, and tools for our business.
    To ensure success as a Laravel developer you should be adept at utilizing Laravel's GUI and be able to design a PHP application from start to finish. A top-notch Laravel developer will be able to leverage their expertise and experience of the framework to independently produce complete solutions in a short turnaround time.`,
    skillsRequired: ['Python', 'Java'],
  };

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
          onClick={() => navigate(AppRoutes.ReceivedApplication)}
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
            Received Applications
          </Typography>
          <ArrowCircleRightRoundedIcon
            color="primary"
            sx={{
              fontSize: '1.5rem',
              color: 'primary',
            }}
          />
        </Box>

        <Typography
          sx={{
            fontSize: '1rem',
            lineHeight: '1.9rem',
          }}
        >
          <strong>Job Category:</strong> {testProps.jobCategory} <br />
          <strong>Job Type:</strong> {testProps.jobType} <br />
          <strong>Location:</strong> {testProps.location} <br />
          <strong>Post Created:</strong>{' '}
          {testProps.postCreated.toLocaleDateString()} <br />
          <strong>Application Deadline:</strong>{' '}
          {testProps.applicationDeadline.toLocaleDateString()}
        </Typography>

        {/* Job description section */}
        <Typography
          sx={{
            fontSize: '1rem',
            mt: '1rem',
          }}
        >
          <strong>Job description</strong>
          <br />
          {testProps.jobDescription}
        </Typography>

        {/* Skills required section */}
        <Typography
          sx={{
            fontSize: '1rem',
            mt: '1rem',
          }}
        >
          <strong>Skills required</strong>
          <ul>
            {testProps.skillsRequired.map((skill, index) => (
              <li key={index}>{skill}</li>
            ))}
          </ul>
        </Typography>
      </Box>
    </Page>
  );
};
