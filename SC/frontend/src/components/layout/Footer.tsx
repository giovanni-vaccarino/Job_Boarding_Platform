import { Box, Typography } from '@mui/material';

export const Footer = () => (
  <Box
    component="footer"
    sx={{
      m: 0,
      p: 2,
      width: '100%',
      textAlign: 'center',
      bgcolor: 'primary.main',
      color: 'common.white',
    }}
  >
    <Typography
      variant="body2"
      sx={{
        fontWeight: 'bold',
        textTransform: 'uppercase',
        letterSpacing: '0.1em',
      }}
    >
      SOFTWARE ENGINEERING 2 - Politecnico di Milano
    </Typography>
  </Box>
);
