import { Box, Typography, TextField } from '@mui/material';
import { Question } from '../../models/company/company.ts';

export const OpenQuestion = (props: Question) => {
  console.log(props);

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        width: '100%',
        marginTop: '5%',
      }}
    >
      <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold' }}>
        {props.title}
      </Typography>
      <Box sx={{ marginTop: 2 }}>
        <TextField
          label="Your Answer"
          variant="outlined"
          fullWidth
          multiline
          rows={4}
          placeholder="Type your answer here..."
        />
      </Box>
    </Box>
  );
};
