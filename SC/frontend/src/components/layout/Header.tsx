import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';

export const Header = () => {
  const navigate = useNavigateWrapper();

  return (
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
          sx={{ fontWeight: 'bold', color: 'primary.main', cursor: 'pointer' }}
          onClick={() => navigate(AppRoutes.Home)}
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
            onClick={() => navigate(AppRoutes.Home)}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
            }}
          >
            Offers
          </Typography>
          <Typography
            onClick={() => navigate(AppRoutes.Matches)}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
            }}
          >
            Matches
          </Typography>
          <Typography
            onClick={() => navigate(AppRoutes.Activity)}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
            }}
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
          onClick={() => navigate(AppRoutes.Login)}
        >
          Login
        </Button>
      </Toolbar>
    </AppBar>
  );
};
