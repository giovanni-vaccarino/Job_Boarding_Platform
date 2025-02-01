import { Box, Button, Snackbar } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { OpenQuestion } from '../components/online-assessment-components/OpenQuestion.tsx';
import { MultipleChoiceQuestion } from '../components/online-assessment-components/MultipleChoiceQuestion.tsx';
import { TrueFalseQuestion } from '../components/online-assessment-components/TrueFalseQuestion.tsx';
import { StartPopup } from '../components/popup/StartPopup.tsx';
import { useLoaderData, useParams } from 'react-router-dom';
import {
  AllAnswersResponse,
  AnswerResponse,
  Question,
} from '../models/company/company.ts';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import { useState } from 'react';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IInternshipApi } from '../core/API/internship/IInternshipApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';
import { AppRoutes } from '../router.tsx';

export const OnlineAssessment = () => {
  const questions = useLoaderData() as Question[];

  const { internshipId, applicationId } = useParams();
  console.log(
    `Internship ID: ${internshipId}, Application ID: ${applicationId}`
  );

  const [answers, setAnswers] = useState<
    Record<string, string | string[] | boolean>
  >({});

  const handleAnswerChange = (
    id: string,
    value: string | string[] | boolean
  ) => {
    setAnswers((prevAnswers) => ({
      ...prevAnswers,
      [id]: value,
    }));
  };

  const renderQuestion = (question: Question) => {
    switch (question.questionType.toString()) {
      case 'OpenQuestion':
        return (
          <OpenQuestion
            id={question.id}
            title={question.title}
            options={question.options}
            questionType={question.questionType}
            onChange={(value) =>
              handleAnswerChange(question.id.toString(), value)
            } // Pass value to handler
          />
        );
      case 'MultipleChoice':
        return (
          <MultipleChoiceQuestion
            id={question.id}
            title={question.title}
            options={question.options}
            questionType={question.questionType}
            onChange={(value) =>
              handleAnswerChange(question.id.toString(), value)
            }
          />
        );
      case 'TrueOrFalse':
        return (
          <TrueFalseQuestion
            id={question.id}
            title={question.title}
            options={question.options}
            questionType={question.questionType}
            onChange={(value) =>
              handleAnswerChange(question.id.toString(), value)
            }
          />
        );
      default:
        return null;
    }
  };

  const isLogged = useAppSelector((s) => s.auth.loggedIn);

  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  const internshipApi = useService<IInternshipApi>(ServiceType.InternshipApi);

  const authState = useAppSelector((state) => state.auth);
  const studentId = authState.profileId;

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <Page>
      <StartPopup />
      <TitleHeader title={'Online Assessment'} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          width: '50%',
          marginBottom: '1rem',
        }}
      >
        {questions.map(renderQuestion)}
      </Box>
      <Button
        variant="contained"
        color="primary"
        disabled={!isLogged}
        onClick={async () => {
          // FUNCTION TO MAP MULTIPLE CHOICE ANSWERES TO NUMERICAL ID
          const mapAnswers = (
            questions: Question[],
            answers: Record<string, string | string[] | boolean>
          ) => {
            return Object.fromEntries(
              Object.entries(answers).map(([id, value]) => {
                const question = questions.find((q) => q.id.toString() === id);
                if (!question) return [id, value];

                  if (
                  question.questionType.toString() === 'MultipleChoice' &&
                  Array.isArray(value)
                ) {
                  const optionMap = Object.fromEntries(
                    question.options.map((opt, i) => [opt, i.toString()])
                  );
                  return [id, value.map((ans) => optionMap[ans] ?? ans)];
                }

                return [id, value]; // Copy everything else unchanged
              })
            );
          };

          const answerResponse: AnswerResponse[] = Object.entries(
            mapAnswers(questions, answers)
          ).map(([id, value]) => ({
            questionId: Number(id), // Ensure questionId is a number
            answer: Array.isArray(value) ? value : value ? [String(value)] : [], // Convert value to string array if necessary
          }));

          const answersResponse: AllAnswersResponse = {
            questions: answerResponse,
          };

          try {
            const res = await internshipApi.postAnswer(
              applicationId as string,
              answersResponse,
              studentId as string
            );
            console.log(res);
            dispatch(
              appActions.global.setConfirmMessage({
                newMessage: 'Answers sent successfully',
              })
            );
            navigate(AppRoutes.ConfirmPage);
          } catch (error) {
            // @ts-ignore
            const errorMessage = error.message.split('\\r')[0];
            console.error('Full error object:', JSON.stringify(error, null, 2));
            setSnackbarMessage(errorMessage);
            setSnackbarOpen(true);
          }
        }}
        sx={{
          textTransform: 'none',
          borderRadius: 2,
          fontSize: '1.15rem',
          px: '1.7rem',
        }}
      >
        Send
      </Button>

      <Box sx={{ marginTop: '2rem' }}> </Box>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        message={snackbarMessage}
        sx={{
          '& .MuiSnackbarContent-root': {
            backgroundColor: 'red',
            fontSize: '18px',
            padding: '16px',
          },
        }}
      />
    </Page>
  );
};
