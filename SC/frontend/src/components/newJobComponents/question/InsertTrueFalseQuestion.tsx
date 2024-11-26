import { useState } from 'react';
import {
  Box,
  TextField,
  Typography,
  Button,
  Radio,
  RadioGroup,
  FormControlLabel,
} from '@mui/material';

export const InsertTrueFalseQuestion = () => {
  const [isVisible, setIsVisible] = useState(true); // Control visibility of the question section
  const [textField, setTextField] = useState('');
  const [selectedOption, setSelectedOption] = useState('True'); // Manage selected option

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
            <Typography>True-False</Typography>
          </Box>
          <Button
            onClick={() => setIsVisible(false)}
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
        {/* True-False Section */}
        <RadioGroup
          value={selectedOption}
          onChange={(e) => setSelectedOption(e.target.value)}
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: '1rem',
            marginTop: '1rem',
          }}
        >
          <FormControlLabel
            value="True"
            control={<Radio />}
            label={<Typography sx={{ color: '#000' }}>True</Typography>}
          />
          <FormControlLabel
            value="False"
            control={<Radio />}
            label={<Typography sx={{ color: '#000' }}>False</Typography>}
          />
        </RadioGroup>
      </Box>
    )
  );
};
