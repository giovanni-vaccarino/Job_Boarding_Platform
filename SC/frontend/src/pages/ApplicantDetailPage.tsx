import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Typography, Button, Stack } from '@mui/material';
import { RowComponent } from '../components/profileComponents/RowComponent.tsx';
import { ViewFeedback } from '../components/applicantDetailPage/ViewFeedback.tsx';

export interface ApplicantDetailPageProps {
  nameApplicant: string;
}

export const ApplicantDetailPage =   (props: ApplicantDetailPageProps) => {
  const feedbackMockUp = [
    { feedbackText: 'Great attention to detail.', rating: 5 },
    { feedbackText: 'Needs improvement in communication.', rating: 3 },
    { feedbackText: 'Excellent technical skills.', rating: 4 },
  ];
  return (
    <Page>
      <TitleHeader title={props.nameApplicant} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          width: '90%',
          alignItems: 'flex-start',
          justifyContent: 'flex-start',
        }}
      >
        <Box
          sx={{
            marginTop: '3%',
            display: 'flex',
            flexDirection: 'column',
            gap: 2,
            marginBottom: '5%',
          }}
        >
          <RowComponent label="CV:" value="" buttons={['view']} />
          <RowComponent
            label="Skills:"
            value="Python, Java, C++"
            buttons={[]}
          />
        </Box>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: 6,
          }}
        >
          <Typography sx={{ fontSize: '2.0rem', fontWeight: '500' }}>
            Feedback
          </Typography>
          {feedbackMockUp.map((feedback, index) => (
            <Box key={index} sx={{ display: 'flex', flexDirection: 'column' }}>
              <Typography
                sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}
              >{`${index + 1})`}</Typography>
              <ViewFeedback
                feedbackText={feedback.feedbackText}
                rating={feedback.rating}
              />
            </Box>
          ))}
        </Box>
        <Box sx = {{
          alignSelf : 'center',
          display : 'flex',
          flexDirection : 'row',
          marginTop : '5%',
          alignItems : 'center',
          marginBottom : '5%'
        }}>
          <Stack spacing={2} direction="row"  >
            <Button variant="contained">Invite</Button>
          </Stack>
        </Box>
      </Box>
    </Page>
  );
};
