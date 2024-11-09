import { Box, CircularProgress } from '@mui/material';

export const LoadingSpinner = () => (
  <Box
    sx={{
      m: 'auto',
    }}
  >
    <CircularProgress size={50} />
  </Box>
);
