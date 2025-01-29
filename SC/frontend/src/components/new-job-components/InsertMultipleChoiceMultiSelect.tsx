import { useState } from 'react';
import { Box, Select, MenuItem, Checkbox, Typography, ListItemText } from '@mui/material';

export interface InsertMultipleChoiceProps {
  titleMultipleChoice: string;
  isRequired: boolean;
  label: string;
  options: string[];
  selectedValues: string[]; // Added prop to track selected values
  onChange: (value: string[]) => void;
}

export const InsertMultipleChoiceMultiSelect = (props: InsertMultipleChoiceProps) => {
  const [selectedChoices, setSelectedChoices] = useState<string[]>(props.selectedValues);

  const handleChange = (event: any) => {
    const {
      target: { value },
    } = event;

    const newValue = typeof value === 'string' ? value.split(',') : value;
    setSelectedChoices(newValue);
    props.onChange(newValue);
  };

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
        multiple
        value={props.selectedValues} // Using prop instead of local state
        onChange={handleChange}
        renderValue={(selected) => selected.join(', ')} // Show selected values
        variant={'outlined'}
      >
        {props.options.map((option) => (
          <MenuItem key={option} value={option}>
            <Checkbox checked={props.selectedValues.includes(option)} />
            <ListItemText primary={option} />
          </MenuItem>
        ))}
      </Select>
    </Box>
  );
};
