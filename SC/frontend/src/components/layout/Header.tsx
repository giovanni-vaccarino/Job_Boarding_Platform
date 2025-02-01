import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  Avatar,
} from '@mui/material';
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
  const isLogged = useAppSelector((s) => s.auth.loggedIn);
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
            dispatch(appActions.global.setHomePageTab({ newTab: Tab.Offers }));
          }}
        >
          S&C
        </Typography>

        <Box
          sx={{
            display: 'flex',
            gap: 5,
            marginLeft:
              authState.profileType === TypeProfile.Company ? '-5rem' : '0',
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
              if (isLogged) {
                navigate(AppRoutes.Matches, { id: authState.profileId || '' });
                dispatch(
                  appActions.global.setHomePageTab({ newTab: Tab.Matches })
                );
              } else {
                navigate(AppRoutes.Login);
              }
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
              if (isLogged) {
                navigate(AppRoutes.Activity, {
                  id: authState.profileId || '',
                });
                dispatch(
                  appActions.global.setHomePageTab({ newTab: Tab.Activity })
                );
              } else {
                navigate(AppRoutes.Login);
              }
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

        {isLogged == false && (
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
        )}
        {isLogged == true && (
          <Avatar
            src="/broken-image.jpg"
            onClick={() =>
              navigate(AppRoutes.Profile, {
                id: authState.profileId || '',
              })
            }
          />
        )}
      </Toolbar>
    </AppBar>
  );
};
