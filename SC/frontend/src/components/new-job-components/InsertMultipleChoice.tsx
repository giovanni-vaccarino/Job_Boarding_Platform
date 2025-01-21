import { useState } from 'react';
import { Box, Select, MenuItem, Typography } from '@mui/material';

export interface InsertMultipleChoiceProps {
  titleMultipleChoice: string;
  isRequired: boolean;
  label: string;
  options: string[];
}

export const InsertMultipleChoice = (props: InsertMultipleChoiceProps) => {
  const [choice, setChoice] = useState('');

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Typography sx={{ fontWeight: 'bold', mb: '0.5rem', ml: '0.5rem' }}>
        {props.titleMultipleChoice}
      </Typography>
      <Select
        required={props.isRequired}
        value={choice}
        label={props.label}
        onChange={(e) => {
          setChoice(e.target.value as string);
        }}
        variant={'outlined'}
      >
        {props.options.map((choice) => (
          <MenuItem value={choice}>{choice}</MenuItem>
        ))}
      </Select>
    </Box>
  );
};
