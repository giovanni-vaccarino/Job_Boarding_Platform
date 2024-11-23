import { Box } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { OpenQuestion } from '../components/online-assessment-components/OpenQuestion.tsx';
import { MultipleChoiceQuestion } from '../components/online-assessment-components/MultipleChoiceQuestion.tsx';

export const OnlineAssessment = () => {
  return (
    <Page>
        <TitleHeader title={'Online Assessment'} />
        <Box sx = {{display : 'flex',
          flexDirection : 'column',
          width: '50%'}}>
          <OpenQuestion />
          <MultipleChoiceQuestion></MultipleChoiceQuestion>
        </Box>
    </Page>
  );
}