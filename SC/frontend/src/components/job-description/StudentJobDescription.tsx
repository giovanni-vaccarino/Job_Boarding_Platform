import { Box, Button } from '@mui/material';
import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { AppRoutes } from '../../router.tsx';
import {
  JobDescriptionCore,
  JobDescriptionProps,
} from './JobDescriptionCore.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';

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
        <JobDescriptionCore
          jobCategory={props.jobCategory}
          jobType={props.jobType}
          location={props.location}
          postCreated={props.postCreated}
          applicationDeadline={props.applicationDeadline}
          jobDescription={props.jobDescription}
          skillsRequired={props.skillsRequired}
        />

        {/* Button Section */}
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
      </Box>
    </>
  );
};
