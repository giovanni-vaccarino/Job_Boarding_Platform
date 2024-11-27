import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { Tabs } from '../../core/store/slices/tabs.ts';

export const Header = () => {
  const navigate = useNavigateWrapper();
  const tabState = useAppSelector((state) => state.tabs);
  const activeTab = tabState.tabHomePage;
  const dispatch = useAppDispatch();

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
          onClick={() => {
            navigate(AppRoutes.Home);
            dispatch(appActions.tabs.setHomePageTab({ newTab: Tabs.Offers }));
          }}
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
            onClick={() => {
              navigate(AppRoutes.Home);
              dispatch(appActions.tabs.setHomePageTab({ newTab: Tabs.Offers }));
            }}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
              fontWeight: activeTab === Tabs.Offers ? 'bold' : 'normal',
            }}
          >
            Offers
          </Typography>
          <Typography
            onClick={() => {
              navigate(AppRoutes.Matches);
              dispatch(
                appActions.tabs.setHomePageTab({ newTab: Tabs.Matches })
              );
            }}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
              fontWeight: activeTab === Tabs.Matches ? 'bold' : 'normal',
            }}
          >
            Matches
          </Typography>
          <Typography
            onClick={() => {
              navigate(AppRoutes.Activity);
              dispatch(
                appActions.tabs.setHomePageTab({ newTab: Tabs.Activity })
              );
            }}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
              fontWeight: activeTab === Tabs.Activity ? 'bold' : 'normal',
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
