import { Box, Button, Typography } from '@mui/material';
import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { AppRoutes } from '../../router.tsx';
import { JobDescriptionCore } from './JobDescriptionCore.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import {
  ApplicationStatus,
  JobDescriptionProps,
} from '../../models/application/application.ts';
import ArrowCircleRightRoundedIcon from '@mui/icons-material/ArrowCircleRightRounded';
import { CreateFeedback } from './CreateFeedback.tsx';

export const StudentJobDescription = (props: JobDescriptionProps) => {
  const isLogged = useAppSelector((s) => s.auth.loggedIn);

  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  return (
    <>
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

        <JobDescriptionCore
          jobCategory={props.jobCategory}
          jobType={props.jobType}
          location={props.location}
          postCreated={props.postCreated}
          applicationDeadline={props.applicationDeadline}
          jobDescription={props.jobDescription}
          skillsRequired={props.skillsRequired}
        />

        {props.status === ApplicationStatus.NotApplied && (
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'center',
            }}
          >
            <Button
              variant="contained"
              color="primary"
              disabled={!isLogged}
              onClick={() => {
                dispatch(
                  appActions.global.setConfirmMessage({
                    newMessage: 'Application Sent Successfully',
                  })
                );
                navigate(AppRoutes.ConfirmPage);
              }}
              sx={{
                textTransform: 'none',
                borderRadius: 2,
                fontSize: '1.15rem',
                px: '1.7rem',
              }}
            >
              Apply
            </Button>
          </Box>
        )}

        {props.status === ApplicationStatus.Ongoing &&
          props.feedbackSelectable && (
            <>
              <CreateFeedback />
            </>
          )}
      </Box>
    </>
  );
};
