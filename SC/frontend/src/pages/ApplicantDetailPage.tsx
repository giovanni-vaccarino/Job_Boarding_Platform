import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, Stack, Typography } from '@mui/material';
import { RowComponent } from '../components/profile-components/RowComponent.tsx';
import { ViewFeedback } from '../components/applicant-detail-page/ViewFeedback.tsx';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import {ApplicantDetailsProps} from "../models/student/student.ts";

const feedbackMockUp = [
  { feedbackText: 'Great attention to detail.', rating: 5 },
  { feedbackText: 'Needs improvement in communication.', rating: 3 },
  { feedbackText: 'Excellent technical skills.', rating: 4 },
];

export const ApplicantDetailPage = (props: ApplicantDetailsProps) => {
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  return (
    <Page>
      <TitleHeader title={props.student.name} />
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
          />
          <RowComponent
            label="Skills:"
            value="Python, Java, C++"
            buttons={[]}
            onFieldChange={() => {}}
          />
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
            Feedback
          </Typography>
          {feedbackMockUp.map((feedback, index) => (
            <Box key={index} sx={{ display: 'flex', flexDirection: 'column' }}>
              <Typography
                sx={{ fontSize: '1.2rem', fontWeight: 'bold' }}
              >{`${index + 1})`}</Typography>
              <ViewFeedback
                feedbackText={feedback.feedbackText}
                rating={feedback.rating}
              />
            </Box>
          ))}
        </Box>
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
            <Button
              variant="contained"
              onClick={() => {
                dispatch(
                  appActions.global.setConfirmMessage({
                    newMessage: 'Invite sent',
                  })
                );
                navigate(AppRoutes.ConfirmPage);
              }}
              sx={{
                fontSize: '1rem',
                fontWeight: 'bold',
                borderRadius: '8px',
              }}
            >
              Invite
            </Button>
          </Stack>
        </Box>
      </Box>
    </Page>
  );
};
