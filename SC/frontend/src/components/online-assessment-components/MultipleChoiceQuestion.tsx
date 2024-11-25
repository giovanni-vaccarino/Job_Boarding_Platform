import { useState } from 'react';
import {
  Checkbox,
  FormControlLabel,
  Box,
  Typography,
  FormGroup,
} from '@mui/material';

export const MultipleChoiceQuestion = () => {
  const [checked, setChecked] = useState({});

  const questionsMockData = {
    question: 'What are your favorite colors?',
    options: ['Red', 'Blue', 'Green', 'Yellow'],
  };

  const handleChange = (event: { target: { name: any; checked: any } }) => {
    const { name, checked } = event.target;
    setChecked((prevState) => ({
      ...prevState,
      [name]: checked,
    }));
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}>
        {questionsMockData.question}
      </Typography>
      <FormGroup>
        {questionsMockData.options.map((option, index) => (
          <FormControlLabel
            key={index}
            control={
              <Checkbox
                checked={checked[option] || false} // Check the individual option
                onChange={handleChange}
                name={option} // Use the option as the name
              />
            }
            label={option}
          />
        ))}
      </FormGroup>
    </Box>
  );
};
