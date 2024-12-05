import { useState } from 'react';
import { Select, Box, MenuItem } from '@mui/material/';
import { Typography } from '@mui/material';

export const TrueFalseQuestion = () => {
  const [checked, setChecked] = useState();

  const handleChange = (event: { target: { name: any; checked: any } }) => {
    setChecked(event.target.value);
  };

  return (
    <Box sx={{ width: 'auto', display: 'flex', flexDirection: 'column' }}>
      <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}>
        Insert your answer
      </Typography>
      <Select
        variant="outlined"
        id="Answer"
        value={checked}
        onChange={handleChange}
      >
        <MenuItem value="" disabled>
          Select an option
        </MenuItem>
        <MenuItem>True</MenuItem>
        <MenuItem>False</MenuItem>
      </Select>
    </Box>
  );
};
