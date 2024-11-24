import { useState } from 'react'
import { Box, Select, MenuItem, Typography } from '@mui/material';

export interface InsertMultipleChoiceProps {
  titleMultipleChoice : string;
  isRequired : boolean;
  label : string;
}

export const InsertMultipleChoice = (props : InsertMultipleChoiceProps) => {

  const [choice, setChoice] = useState('');

  const handleChange = (event) => {
    console.log("ciao")
    setChoice(event.target.value);
  };

  const MockUpChoice = [
    "London", "Paris"]

  return (
    <Box sx ={{
      display : 'flex',
      flexDirection : 'column',
    }} >
        <Typography>
          {props.titleMultipleChoice}
        </Typography>
        <Select
          required = {props.isRequired}
          value = {MockUpChoice[0]}
          label={props.label}
          onChange={handleChange}
        >
          {MockUpChoice.map((choice) =>
              (<MenuItem value = {choice}>
                {choice}
              </MenuItem>))}
        </Select>
    </Box>
  );
}