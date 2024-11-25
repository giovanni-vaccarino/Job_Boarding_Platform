import { useState } from 'react';
import { Box, TextField, Typography } from '@mui/material';

export interface InsertTextFieldProps {
  titleTextField: string;
  isRequired: boolean;
  label: string;
}

export const InsertTextField = (props: InsertTextFieldProps) => {
  const [textField, setTextField] = useState('');

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Typography>{props.titleTextField}</Typography>
      <TextField
        required={props.isRequired}
        id={props.label}
        onChange={(e) => {
          setTextField(e.target.value);
        }}
        label={props.label}
        value={textField}
      />
    </Box>
  );
};
