import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { ServiceType } from '../core/ioc/service-type.ts';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { SendVerificationMailDto } from '../models/auth/login.ts';

export const ForgotPasswordSetEmail = () => {
  const [email, setEmail] = useState<string>('');

  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  const authApi = useService<IAuthApi>(ServiceType.AuthApi);

  return (
    <Page>
      <TitleHeader title={'Forgot Password'} />

      <Box
        sx={{
          width: '100%',
          maxWidth: '500px',
          margin: 'auto',
          padding: 3,
          mt: '3rem',
          borderRadius: '8px',
          boxShadow: '0px 2px 8px rgba(0, 0, 0, 0.1)',
          backgroundColor: '#FFFFFF',
        }}
      >
        <Box
          sx={{
            mb: '1rem',
          }}
        >
          <Typography sx={{ fontSize: '1.35rem', fontWeight: '500' }}>
            Email
          </Typography>
          <TextField
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            fullWidth
            variant="outlined"
            placeholder="Email"
            required
            margin="normal"
          />
        </Box>

        <Button
          fullWidth
          variant="contained"
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            textTransform: 'none',
            fontSize: '1rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            marginTop: 2,
            marginBottom: 2,
          }}
          onClick={async () => {
            try {
              const sendResetPassword: SendVerificationMailDto = {
                email: email,
              };
              const res = await authApi.sendResetPassword(sendResetPassword);
              console.log(res);

              dispatch(
                appActions.global.setConfirmMessage({
                  newMessage: 'Email Sent Successfully',
                })
              );
              navigate(AppRoutes.ConfirmPage);
            } catch (error) {
              //TODO PRINT ERR MESSAGE
            }
          }}
        >
          Send Email
        </Button>
      </Box>
    </Page>
  );
};
