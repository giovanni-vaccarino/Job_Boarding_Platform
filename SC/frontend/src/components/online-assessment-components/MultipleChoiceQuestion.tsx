import { useState } from 'react';
import {
  Checkbox,
  FormControlLabel,
  Box,
  Typography,
  FormGroup,
} from '@mui/material';
import { Question } from '../../models/company/company.ts';

export const MultipleChoiceQuestion = (props: Question) => {
  const [checked, setChecked] = useState({});

  const handleChange = (event: { target: { name: any; checked: any } }) => {
    const { name, checked } = event.target;
    setChecked((prevState) => ({
      ...prevState,
      [name]: checked,
    }));

    //TODO da fare un array che controlli se è checked o no per ogni campo

    props.onChange(event.target.name); // Trigger the callback with the input value
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}>
        {props.title}
      </Typography>
      <FormGroup>
        {props.options.map((option, index) => (
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
