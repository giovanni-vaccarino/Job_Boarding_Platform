import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button} from '@mui/material';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useLocation } from 'react-router-dom';
import { Tab } from '../core/store/slices/global.ts';
import { useEffect, useState } from 'react';
import { SendVerificationMailDto, VerifyMailDto } from '../models/auth/login.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';

export const VerifyEmail = () => {
  const authState = useAppSelector((state) => state.auth);
  const isLogged = useAppSelector((s) => s.auth.loggedIn);
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();
  const search = useLocation().search;
  const token = new URLSearchParams(search).get('token');
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);

  const [message, setMessage] = useState('');

  useEffect(() => {
    const handleVerificationMail = async () => {
      const dto: VerifyMailDto = {
        VerificationToken: token,
      };

      try {
        await authApi.verifyMail(dto);
        setMessage("Email Verified!");
        dispatch(appActions.auth.setVerified());
      } catch (error) {
        console.error('Error sending verification mail:', error);
        setMessage("Email Not Verified!");
      }
    }

    handleVerificationMail();
  },[]);

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
        <TitleHeader title={message} />

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
