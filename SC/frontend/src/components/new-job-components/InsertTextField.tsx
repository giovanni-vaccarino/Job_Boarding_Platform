import { Box, TextField, Typography } from '@mui/material';

export interface InsertTextFieldProps {
  titleTextField: string;
  isRequired: boolean;
  label: string;
  value: string;
  onChange: (value: string) => void;
}

export const InsertTextField = (props: InsertTextFieldProps) => {
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let value = e.target.value;

    if (props.label === 'Application Deadline' && value.length >= 10) {
      const date = new Date(value);
      if (!isNaN(date.getTime())) {
        value = date.toISOString().split('T')[0];
      }
    }

    props.onChange(value);
  };

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
        onChange={handleInputChange}
        label={props.label}
        value={props.value}
      />
    </Box>
  );
};
