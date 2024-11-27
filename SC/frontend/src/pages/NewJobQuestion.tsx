import { useState } from 'react';
import { Box, Button, Select, MenuItem } from '@mui/material';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Page } from '../components/layout/Page.tsx';
import { InsertOpenQuestion } from '../components/newJobComponents/question/InsertOpenQuestion.tsx';
import { InsertMultipleChoiceQuestion } from '../components/newJobComponents/question/InsertMultipleChoiceQuestion.tsx';
import { InsertTrueFalseQuestion } from '../components/newJobComponents/question/InsertTrueFalseQuestion.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../router.tsx';
import { appActions, useAppDispatch } from '../core/store';

export const NewJobQuestion = () => {
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  const [selectedQuestionType, setSelectedQuestionType] = useState(''); // Track selected question type
  const [questions, setQuestions] = useState<any[]>([]); // Store added questions

  const handleAddQuestion = () => {
    if (selectedQuestionType === 'True/False') {
      setQuestions([
        ...questions,
        <InsertTrueFalseQuestion key={questions.length} />,
      ]);
    } else if (selectedQuestionType === 'Multiple Choice') {
      setQuestions([
        ...questions,
        <InsertMultipleChoiceQuestion key={questions.length} />,
      ]);
    } else if (selectedQuestionType === 'Open Question') {
      setQuestions([
        ...questions,
        <InsertOpenQuestion key={questions.length} />,
      ]);
    }
    setSelectedQuestionType(''); // Reset selection
  };

  return (
    <Page>
      <TitleHeader title={'Create a Job - Questions'} />

      {/* Display dynamically added questions */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          gap: '2rem',
          marginTop: '2rem',
          width: '80%',
        }}
      >
        {questions}
      </Box>

      {/* Add Question Component */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'left',
          alignItems: 'center',
          gap: '2rem',
          marginTop: '2rem',
          width: '80%',
        }}
      >
        <Select
          value={selectedQuestionType}
          onChange={(e) => setSelectedQuestionType(e.target.value)}
          displayEmpty
          sx={{
            width: '20%',
            backgroundColor: '#f9f9f9',
            borderRadius: '8px',
            '& .MuiOutlinedInput-root': {
              borderRadius: '8px',
            },
          }}
        >
          <MenuItem value="" disabled>
            Select Question Type *
          </MenuItem>
          <MenuItem value="True/False">True/False</MenuItem>
          <MenuItem value="Multiple Choice">Multiple Choice</MenuItem>
          <MenuItem value="Open Question">Open Question</MenuItem>
        </Select>
        <Button
          onClick={handleAddQuestion}
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            fontSize: '1rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            padding: '0.5rem 1.3rem',
            '&:hover': {
              backgroundColor: 'primary.dark',
            },
          }}
        >
          Add Question
        </Button>
      </Box>

      {/* Post Job Button */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'flex-end',
          marginTop: '2rem',
        }}
      >
        <Button
          onClick={() => {
            dispatch(
              appActions.global.setConfirmMessage({
                newMessage: 'New Job Created',
              })
            );
            navigate(AppRoutes.ConfirmPage);
          }}
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            fontSize: '1.2rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            marginBottom: 2,
            padding: '0.5rem 2rem',
            '&:hover': {
              backgroundColor: 'primary.dark',
            },
          }}
        >
          Post Job
        </Button>
      </Box>
    </Page>
  );
};
