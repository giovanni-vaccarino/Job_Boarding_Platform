import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';

export const Header = () => (
  <AppBar
    position="static"
    sx={{
      width: '100%',
      maxWidth: '100%',
      m: 0,
      p: '1rem 3rem',
      bgcolor: 'background.paper',
    }}
  >
    <Toolbar
      variant="dense"
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        width: '100%',
        maxWidth: '100%',
        p: 0,
      }}
    >
      <Typography
        variant="h4"
        sx={{ fontWeight: 'bold', color: 'primary.main' }}
      >
        S&C
      </Typography>

      <Box
        sx={{
          display: 'flex',
          gap: 5,
        }}
      >
        <Typography
          variant="body1"
          sx={{ color: 'primary.main', cursor: 'pointer', fontSize: '1.25rem' }}
        >
          Offers
        </Typography>
        <Typography
          variant="body1"
          sx={{ color: 'primary.main', cursor: 'pointer', fontSize: '1.25rem' }}
        >
          Matches
        </Typography>
        <Typography
          variant="body1"
          sx={{ color: 'primary.main', cursor: 'pointer', fontSize: '1.25rem' }}
        >
          Activity
        </Typography>
      </Box>

      <Button
        variant="contained"
        color="primary"
        sx={{
          textTransform: 'none',
          borderRadius: 2,
          fontWeight: 'bold',
          fontSize: '1.25rem',
          px: '3rem',
        }}
      >
        Login
      </Button>
    </Toolbar>
  </AppBar>
);
