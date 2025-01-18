import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { Tab } from '../../core/store/slices/global.ts';
import { TypeProfile } from '../../models/auth/register.ts';

export const Header = () => {
  const navigate = useNavigateWrapper();
  const activeTab = useAppSelector((state) => state.global.tabHomePage);
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;
  const dispatch = useAppDispatch();

  console.log(authState.profileId);

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
            dispatch(appActions.global.setHomePageTab({ newTab: Tab.Offers }));
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
          {profileType !== TypeProfile.Company && (
            <Typography
              onClick={() => {
                navigate(AppRoutes.Home);
                dispatch(
                  appActions.global.setHomePageTab({ newTab: Tab.Offers })
                );
              }}
              variant="body1"
              sx={{
                color: 'primary.main',
                cursor: 'pointer',
                fontSize: '1rem',
                fontWeight: activeTab === Tab.Offers ? 'bold' : 'normal',
              }}
            >
              Offers
            </Typography>
          )}
          <Typography
            onClick={() => {
              navigate(AppRoutes.Matches, { id: authState.profileId });
              dispatch(
                appActions.global.setHomePageTab({ newTab: Tab.Matches })
              );
            }}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
              fontWeight: activeTab === Tab.Matches ? 'bold' : 'normal',
            }}
          >
            Matches
          </Typography>
          <Typography
            onClick={() => {
              navigate(AppRoutes.Activity, {
                id: authState.profileId,
              });
              dispatch(
                appActions.global.setHomePageTab({ newTab: Tab.Activity })
              );
            }}
            variant="body1"
            sx={{
              color: 'primary.main',
              cursor: 'pointer',
              fontSize: '1rem',
              fontWeight: activeTab === Tab.Activity ? 'bold' : 'normal',
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
