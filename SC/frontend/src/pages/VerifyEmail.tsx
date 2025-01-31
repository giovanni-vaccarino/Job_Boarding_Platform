import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button} from '@mui/material';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useLocation } from 'react-router-dom';
import { Tab } from '../core/store/slices/global.ts';

export const VerifyEmail = () => {
  const authState = useAppSelector((state) => state.auth);
  const isLogged = useAppSelector((s) => s.auth.loggedIn);
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();
  const search = useLocation().search;
  const token = new URLSearchParams(search).get('token');

  if (token === null) {
    navigate(AppRoutes.Profile);
  }

  return (
    <Page>
      <Box
        sx={{
          flexDirection: 'column',
          display: 'flex',
          justifyContent: 'center',
          width: '100%',
          mt: '13rem',
        }}
      >
        <TitleHeader title={"Email Verified!"} />

        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            mt: '5rem',
          }}
        >
          <Button
            onClick={() => {
              if (isLogged) {
                dispatch(
                  appActions.global.setHomePageTab({ newTab: Tab.Activity })
                );
                navigate(AppRoutes.Activity, {
                  id: authState.profileId,
                });
              } else {
                dispatch(
                  appActions.global.setHomePageTab({ newTab: Tab.Offers })
                );
                navigate(AppRoutes.Home);
              }
            }}
            variant="contained"
            color="primary"
            sx={{
              textTransform: 'none',
              borderRadius: 2,
              fontSize: '1.15rem',
              px: '1.7rem',
            }}
          >
            Go back
          </Button>
        </Box>
      </Box>
    </Page>
  );
};
