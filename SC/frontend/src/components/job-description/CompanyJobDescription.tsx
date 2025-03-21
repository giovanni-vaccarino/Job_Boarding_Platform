import { Box, Typography } from '@mui/material';
import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { AppRoutes } from '../../router.tsx';
import { JobDescriptionCore } from './JobDescriptionCore.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import ArrowCircleRightRoundedIcon from '@mui/icons-material/ArrowCircleRightRounded';
import { JobDescriptionProps } from '../../models/application/application.ts';
import { useAppSelector } from '../../core/store';

export const CompanyJobDescription = (props: JobDescriptionProps) => {
  const navigate = useNavigateWrapper();
  const jobDescription = props.jobDescription;

  const authState = useAppSelector((state) => state.auth);

  return (
    <>
      <TitleHeader title={jobDescription.jobTitle} />

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
          onClick={() =>
            navigate(AppRoutes.ReceivedApplications, {
              internshipId: jobDescription.jobId.toString(),
              companyId: authState.profileId || '',
            })
          }
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

        <JobDescriptionCore jobDescription={jobDescription} />
      </Box>
    </>
  );
};
