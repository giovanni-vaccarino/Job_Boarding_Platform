import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, Stack, Typography } from '@mui/material';
import { RowComponent } from '../components/profile-components/RowComponent.tsx';
import { ViewFeedback } from '../components/applicant-detail-page/ViewFeedback.tsx';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useLoaderData, useParams } from 'react-router-dom';
import { ApplicantInfo } from '../models/internship/internship.ts';
import {
  ApplicationStatus,
  UpdateStatusApplicationDto,
} from '../models/application/application.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IMatchApi } from '../core/API/match/IMatchApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';
import { IInternshipApi } from '../core/API/internship/IInternshipApi.ts';

export const ApplicantDetailPage = () => {
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();
  const student = useLoaderData() as ApplicantInfo;
  const applicationStatus = useParams().applicationStatus; // Assuming isApplication is passed as a string in param
  const submissionDate = useParams().submissionDate;
  const applicationId = useParams().applicationId;
  const auth = useAppSelector((state) => state.auth);
  const companyId = auth.profileId;
  const matchId = useParams().matchId;
  const matchApi = useService<IMatchApi>(ServiceType.MatchApi);
  const internshiApi = useService<IInternshipApi>(ServiceType.InternshipApi);

  const handleAccept = async () => {
    if (
      applicationStatus === "LastEvaluation"
    ) {

      dispatch(
        appActions.global.setConfirmMessage({
          newMessage: 'Application accepted',
        })
      );
      const status: UpdateStatusApplicationDto = {
        status: ApplicationStatus.Accepted,
      };
      console.log(status);
      const res = await internshiApi.updateApplicationStatus(
        applicationId as string,
        status,
        companyId as string
      );
      navigate(AppRoutes.ConfirmPage);
    } else if (
      applicationStatus == "Screening"
    ) {

      dispatch(
        appActions.global.setConfirmMessage({
          newMessage: 'Application accepted - waiting for online assessment',
        })
      );
      const status: UpdateStatusApplicationDto = {
        status: ApplicationStatus.OnlineAssessment,
      };
      const res = await internshiApi.updateApplicationStatus(
        applicationId as string,
        status,
        companyId as string
      );
      console.log(res);

      navigate(AppRoutes.ConfirmPage);
    }
  };

  const handleReject = async () => {
    dispatch(
      appActions.global.setConfirmMessage({
        newMessage: 'Application rejected',
      })
    );
    const status: UpdateStatusApplicationDto = {
      status: ApplicationStatus.Accepted,
    };
    const res = await internshiApi.updateApplicationStatus(
      applicationId as string,
      status,
      companyId as string
    );
    console.log(res);

    navigate(AppRoutes.ConfirmPage);
  };

  const handleInvite = async () => {
    dispatch(
      appActions.global.setConfirmMessage({
        newMessage: 'Invite sent',
      })
    );
    //TODO add matchId in the Database
    const inputToApi = {
      matchId: matchId,
      companyId: companyId,
    };

    const res = await matchApi.postInviteStudent(
      inputToApi.matchId as string,
      inputToApi.companyId as string
    );
    console.log(res);
    navigate(AppRoutes.ConfirmPage);
  };

  console.log(student.answers[0]);

  return (
    <Page>
      <TitleHeader title={student.name} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          width: '50%',
          alignItems: 'flex-start',
          justifyContent: 'flex-start',
        }}
      >
        <Box
          sx={{
            marginTop: '3%',
            display: 'flex',
            flexDirection: 'column',
            gap: 1,
            marginBottom: '5%',
          }}
        >
          <RowComponent
            label="CV:"
            value=""
            buttons={['view']}
            onFieldChange={() => {}}
            studentIdToRetrieveCV={student.studentId}
          />
          <RowComponent
            label="Skills:"
            value={student.skills}
            buttons={[]}
            onFieldChange={() => {}}
          />
          {applicationStatus != 'null' && (
              <RowComponent
                label="Submission Date:"
                value={submissionDate as string}
                buttons={[]}
                onFieldChange={() => {}}
              />
            ) && (
              <RowComponent
                label="Status:"
                value={applicationStatus?.toString() as string}
                buttons={[]}
                onFieldChange={() => {}}
              />
            )}
        </Box>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            mt: '2rem',
            gap: 3,
          }}
        >
          <Typography sx={{ fontSize: '2.0rem', fontWeight: '500' }}>
            Feedback:
          </Typography>
          {student.feedbacks?.map((feedback, index) => (
            <Box key={index} sx={{ display: 'flex', flexDirection: 'column' }}>
              <Typography
                sx={{ fontSize: '1.2rem', fontWeight: 'bold' }}
              >{`${index + 1})`}</Typography>
              <ViewFeedback
                feedbackText={feedback.text}
                rating={feedback.rating}
              />
            </Box>
          ))}
        </Box>
        {applicationStatus ===
          ApplicationStatus[ApplicationStatus.LastEvaluation] && (
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              mt: '2rem',
              gap: 3,
            }}
          >
            <Typography sx={{ fontSize: '2.0rem', fontWeight: '500' }}>
              Assessment answers:
            </Typography>
            {student.answers.map((assessment, index) => (
              <Box
                key={index}
                sx={{ display: 'flex', flexDirection: 'column' }}
              >
                <Typography
                  sx={{ fontSize: '1.2rem', fontWeight: 'bold' }}
                >{`${index + 1})`}</Typography>
                <Typography sx={{ fontSize: '1.2rem', fontWeight: '500' }}>
                  {
                    //@ts-ignore
                    assessment?.question?.title || assessment?.questions?.title
                  }
                </Typography>
                <Typography sx={{ fontSize: '1rem', fontWeight: '200' }}>
                  {assessment?.answer}
                </Typography>
              </Box>
            ))}
          </Box>
        )}
        <Box
          sx={{
            alignSelf: 'center',
            display: 'flex',
            flexDirection: 'row',
            marginTop: '5%',
            alignItems: 'center',
            marginBottom: '5%',
            width: '2rem',
          }}
        >
          <Stack spacing={2} direction="row">
            {applicationStatus ===
              ApplicationStatus[ApplicationStatus.LastEvaluation] ||
            applicationStatus ===
              ApplicationStatus[ApplicationStatus.Screening] ? (
              <>
                <Button
                  variant="contained"
                  onClick={handleAccept}
                  sx={{
                    fontSize: '1rem',
                    fontWeight: 'bold',
                    borderRadius: '8px',
                  }}
                >
                  Accept
                </Button>
                <Button
                  variant="outlined"
                  onClick={handleReject}
                  sx={{
                    fontSize: '1rem',
                    fontWeight: 'bold',
                    borderRadius: '8px',
                  }}
                >
                  Reject
                </Button>
              </>
            ) : (
              applicationStatus == 'null' && (
                <Button
                  variant="contained"
                  onClick={handleInvite}
                  sx={{
                    fontSize: '1rem',
                    fontWeight: 'bold',
                    borderRadius: '8px',
                  }}
                >
                  Invite
                </Button>
              )
            )}
          </Stack>
        </Box>
      </Box>
    </Page>
  );
};
