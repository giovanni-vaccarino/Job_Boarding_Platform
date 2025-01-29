import { useEffect, useState } from 'react';
import { Box, Button, Checkbox, TextField, Typography } from '@mui/material';
import { AddQuestionDto } from '../../../models/internship/internship.ts';
import { QuestionType } from '../../../models/company/company.ts';

interface MultipleChoiceAnswer {
  id: number;
  value: string;
  selected: boolean;
}

interface InsertMultipleChoiceQuestionProps {
  id: number;
  onSave: (id: number, question: AddQuestionDto) => void;
  onRemove: (id: number) => void;
}

export const InsertMultipleChoiceQuestion = ({
  id,
  onSave,
  onRemove,
}: InsertMultipleChoiceQuestionProps) => {
  const [isVisible, setIsVisible] = useState(true);
  const [textField, setTextField] = useState('');
  const [answers, setAnswers] = useState<MultipleChoiceAnswer[]>([
    { id: 1, value: '', selected: false },
  ]);

  const handleAddAnswer = () => {
    setAnswers([
      ...answers,
      { id: answers.length + 1, value: '', selected: false },
    ]);
  };

  const handleRemoveAnswer = (answerId: number) => {
    setAnswers(answers.filter((answer) => answer.id !== answerId));
  };

  const handleAnswerChange = (answerId: number, value: string) => {
    setAnswers(
      answers.map((answer) =>
        answer.id === answerId ? { ...answer, value } : answer
      )
    );
  };

  const handleToggleSelect = (answerId: number) => {
    setAnswers(
      answers.map((answer) =>
        answer.id === answerId
          ? { ...answer, selected: !answer.selected }
          : answer
      )
    );
  };

  const handleRemoveQuestion = () => {
    if (onRemove) {
      onRemove(id);
    }
    setIsVisible(false);
  };

  const handleSave = () => {
    const question: AddQuestionDto = {
      Title: textField,
      QuestionType: QuestionType.MultipleChoice,
      Options: answers.map((answer) => answer.value),
    };

    onSave(id, question);
  };

  useEffect(() => {
    handleSave();
  }, [textField, answers]);

  return (
    isVisible && (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          gap: '1rem',
          border: '1px solid #e0e0e0',
          borderRadius: '8px',
          padding: '1.5rem',
          width: '100%',
        }}
      >
        {/* Question Type */}
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            gap: '1rem',
          }}
        >
          <Typography sx={{ fontWeight: 'bold', fontSize: '1.2rem' }}>
            Question Type
          </Typography>
          <Box
            sx={{
              minWidth: '10rem',
              backgroundColor: '#f9f9f9',
              borderRadius: '8px',
              padding: '0.5rem',
              fontWeight: 'bold',
              color: '#9e9e9e',
            }}
          >
            <Typography>Multiple Choice</Typography>
          </Box>
          <Button
            onClick={handleRemoveQuestion}
            sx={{
              minWidth: '2.5rem',
              height: '2.5rem',
              backgroundColor: 'primary.main',
              color: '#fff',
              borderRadius: '8px',
              '&:hover': {
                backgroundColor: 'primary.dark',
              },
            }}
          >
            -
          </Button>
        </Box>
        {/* Text Field Section */}
        <Typography sx={{ fontWeight: 'bold', fontSize: '1.2rem' }}>
          Write here your question:
        </Typography>
        <TextField
          required={true}
          id="Question"
          onChange={(e) => setTextField(e.target.value)}
          label="Question"
          value={textField}
          sx={{
            width: '100%',
          }}
        />
        {/* Multiple Answers Section */}
        {answers.map((answer) => (
          <Box
            key={answer.id}
            sx={{
              display: 'flex',
              alignItems: 'center',
              gap: '1rem',
            }}
          >
            <Checkbox
              checked={answer.selected}
              onChange={() => handleToggleSelect(answer.id)}
            />
            <TextField
              required={true}
              label="Possible Answer"
              value={answer.value}
              onChange={(e) => handleAnswerChange(answer.id, e.target.value)}
              sx={{
                flexGrow: 1,
              }}
            />
            <Button
              onClick={() => handleRemoveAnswer(answer.id)}
              sx={{
                minWidth: '2.5rem',
                height: '2.5rem',
                backgroundColor: 'primary.main',
                color: '#fff',
                borderRadius: '8px',
                '&:hover': {
                  backgroundColor: 'primary.dark',
                },
              }}
            >
              -
            </Button>
          </Box>
        ))}
        <Box sx={{ width: '2.5rem', pl: '3.5rem' }}>
          <Button
            onClick={handleAddAnswer}
            sx={{
              height: '2.5rem',
              backgroundColor: 'primary.main',
              color: '#fff',
              borderRadius: '8px',
              '&:hover': {
                backgroundColor: 'primary.dark',
              },
            }}
          >
            +
          </Button>
        </Box>
      </Box>
    )
  );
};
