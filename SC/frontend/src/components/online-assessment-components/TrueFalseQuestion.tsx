import { useState } from 'react';
import { Select, Box, MenuItem, SelectChangeEvent } from '@mui/material/';
import { Typography } from '@mui/material';
import { Question } from '../../models/company/company.ts';

export const TrueFalseQuestion = (props: Question) => {
  const [checked, setChecked] = useState<boolean | undefined>(undefined);

  const handleChange = (event: SelectChangeEvent<string>) => {
    const value = event.target.value === 'true' ? true : false; // Convert to boolean
    setChecked(value); // Update internal state
    props.onChange(event.target.value); // Notify parent of the selected value
  };

  return (
    <Box sx={{ width: 'auto', display: 'flex', flexDirection: 'column' }}>
      <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}>
        {props.title}
      </Typography>
      <Select
        variant="outlined"
        id="Answer"
        value={checked !== undefined ? checked.toString() : ''}
        onChange={handleChange}
        displayEmpty
      >
        <MenuItem value="" disabled>
          Select an option
        </MenuItem>
        <MenuItem value="true">True</MenuItem>
        <MenuItem value="false">False</MenuItem>
      </Select>
    </Box>
  );
};
