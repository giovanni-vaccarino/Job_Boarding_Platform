import { Box, Button } from '@mui/material';
import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { AppRoutes } from '../../router.tsx';
import { JobDescriptionCore } from './JobDescriptionCore.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { JobDescriptionProps } from '../../models/application/application.ts';
import { useService } from '../../core/ioc/ioc-provider.tsx';
import { IInternshipApi } from '../../core/API/internship/IInternshipApi.ts';
import { ServiceType } from '../../core/ioc/service-type.ts';
import { ApplyToInternshipInput } from '../../models/internship/internship.ts';

export const StudentJobDescription = (props: JobDescriptionProps) => {
  const jobDescription = props.jobDescription;

  const isLogged = useAppSelector((s) => s.auth.loggedIn);

  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  const internshipApi = useService<IInternshipApi>(ServiceType.InternshipApi);

  const authState = useAppSelector((state) => state.auth);
  const studentId = authState.profileId;

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
        <JobDescriptionCore jobDescription={jobDescription} />

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
            onClick={async () => {
              const inputPostApplyToInternship: ApplyToInternshipInput = {
                studentId: studentId?.toString(),
                internshipId: props.jobDescription.jobId.toString(),
              };
              const res = await internshipApi.postApplyToInternship(
                inputPostApplyToInternship
              );
              console.log(res);
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
