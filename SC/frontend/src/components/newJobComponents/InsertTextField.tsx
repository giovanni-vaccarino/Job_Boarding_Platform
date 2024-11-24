import { useState } from 'react'
import { Box, TextField, Typography } from '@mui/material';

export interface InsertTextFieldProps {
  titleTextField: string;
  isRequired: boolean;
  label : string;
}

export const InsertTextField = (props : InsertTextFieldProps) => {

  const [textField, setTextField] = useState("")

  const handleSetTextField = (event) =>
  {
    setTextField(event.target.value)
  }

  return (
    <Box sx ={{
      display : 'flex',
      flexDirection : 'column',
    }} >
      <Typography>
        {props.titleTextField}
      </Typography>
      <TextField
        required = {props.isRequired}
        id={props.label}
        onChange={handleSetTextField}
        label={props.label  }
      />
    </Box>
  );
}