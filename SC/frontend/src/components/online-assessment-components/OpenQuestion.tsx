import { Box, Typography, TextField } from '@mui/material';

export const OpenQuestion = () => {
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
        First Question
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
