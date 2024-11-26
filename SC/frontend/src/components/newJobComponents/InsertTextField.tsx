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
      <Typography sx={{ fontWeight: 'bold', mb: '0.5rem', ml: '0.5rem' }}>
        {props.titleTextField}
      </Typography>
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
