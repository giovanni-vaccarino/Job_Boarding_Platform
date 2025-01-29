import { useEffect, useState } from 'react';
import { Box, TextField, Typography, Button } from '@mui/material';
import { AddQuestionDto } from '../../../models/internship/internship.ts';
import { QuestionType } from '../../../models/company/company.ts';

interface InsertOpenQuestionProps {
  id: number; // Unique identifier for the question
  onSave: (id: number, question: AddQuestionDto) => void; // Callback for saving question
  onRemove: (id: number) => void; // Callback for removing the question
}

export const InsertOpenQuestion = ({
  id,
  onSave,
  onRemove,
}: InsertOpenQuestionProps) => {
  const [isVisible, setIsVisible] = useState(true); // Control visibility of the question section
  const [textField, setTextField] = useState(''); // Question text

  // Function to save the question
  const handleSave = () => {
    const question: AddQuestionDto = {
      Title: textField,
      QuestionType: QuestionType.OpenQuestion,
      Options: [], // Open questions don't have options
    };
    onSave(id, question);
  };

  // Call handleSave whenever textField changes
  useEffect(() => {
    if (textField.trim() !== '') {
      handleSave();
    }
  }, [textField]);

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
            Question Type:
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
            <Typography>Open Question</Typography>
          </Box>
          <Button
            onClick={() => {
              setIsVisible(false);
              onRemove(id); // Notify the outer component
            }}
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
          id="Open Question"
          onChange={(e) => setTextField(e.target.value)}
          label="Open Question"
          value={textField}
          sx={{
            width: '100%',
          }}
        />
      </Box>
    )
  );
};
