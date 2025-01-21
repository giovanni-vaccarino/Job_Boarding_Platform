import { Box } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { OpenQuestion } from '../components/online-assessment-components/OpenQuestion.tsx';
import { MultipleChoiceQuestion } from '../components/online-assessment-components/MultipleChoiceQuestion.tsx';
import { TrueFalseQuestion } from '../components/online-assessment-components/TrueFalseQuestion.tsx';
import { StartPopup } from '../components/popup/StartPopup.tsx';
import { useLoaderData } from 'react-router-dom';
import { Question, QuestionType } from '../models/company/company.ts';

const renderQuestion = (question: Question) => {
  switch (question.questionType.toString()) {
    case 'OpenQuestion':
      return (
        <OpenQuestion
          id={question.id}
          title={question.title}
          options={question.options}
          questionType={question.questionType}
        />
      );
    case 'MultipleChoice':
      return (
        <MultipleChoiceQuestion
          id={question.id}
          title={question.title}
          options={question.options}
          questionType={question.questionType}
        />
      );
    case 'TrueOrFalse':
      return (
        <TrueFalseQuestion
          id={question.id}
          title={question.title}
          options={question.options}
          questionType={question.questionType}
        />
      );
    default:
      return null;
  }
};

export const OnlineAssessment = () => {
  const questions = useLoaderData() as Question[];
  console.log(questions);
  return (
    <Page>
      <StartPopup />
      <TitleHeader title={'Online Assessment'} />
      <Box sx={{ display: 'flex', flexDirection: 'column', width: '50%' }}>
        {questions.map(renderQuestion)}
      </Box>
    </Page>
  );
};
