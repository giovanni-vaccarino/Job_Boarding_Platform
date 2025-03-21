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
  const [checkedAnsweres, setCheckedAnsweres] = useState({});

  const handleChange = (event: {
    target: { name: string; checked: boolean };
  }) => {
    const { name, checked } = event.target;

    setCheckedAnsweres((prevState) => {
      const updatedState = { ...prevState, [name]: checked };

      props.onChange(
        Object.keys(updatedState).filter((key) => updatedState[key])
      );

      return updatedState;
    });
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
              // @ts-ignore
              <Checkbox
                checked={checkedAnsweres[option] || false} // Check the individual option
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
