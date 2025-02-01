import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, Snackbar, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';
import { LoginInput } from '../models/auth/login.ts';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { TypeProfile } from '../models/auth/register.ts';

export const Login = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const navigate = useNavigateWrapper();
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);
  const dispatch = useAppDispatch();

  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <Page>
      <TitleHeader title={'Login'} />

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
            id = "emailField"
          />
        </Box>

        <Box
          sx={{
            mb: '1.5rem',
          }}
        >
          <Typography sx={{ fontSize: '1.35rem', fontWeight: '500' }}>
            Password
          </Typography>
          <TextField
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            fullWidth
            variant="outlined"
            placeholder="Password"
            type="password"
            required
            margin="normal"
            id = "passwordField"
          />
        </Box>

        <Button
          fullWidth
          variant="contained"
          id = "loginButton"
          onClick={async () => {
            const loginInput: LoginInput = {
              email: email,
              password: password,
            };

            try {
              const res = await authApi.login(loginInput);

              console.log('idstudente' + res.profileId);

              console.log(res);
              dispatch(appActions.auth.successLogin(res));
              const profileTypeEnum =
                res.profileType.toString() === 'Student'
                  ? TypeProfile.Student
                  : TypeProfile.Company;
              dispatch(
                appActions.auth.setProfileType({ type: profileTypeEnum })
              );

              dispatch(
                appActions.auth.setProfileId({ id: res.profileId.toString() })
              );
              console.log(res.profileId.toString());
              navigate(AppRoutes.Profile, {
                id: res.profileId.toString(),
              });
              setInterval(function () {
                window.location.reload();
              }, 500);
            } catch (error: any) {
              const errorMessage = error.message.split('\\r')[0];
              console.log(error.message);
              console.log('Full error object:', JSON.stringify(error, null, 2));
              setSnackbarMessage(errorMessage);
              setSnackbarOpen(true);
            }
          }}
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
        >
          Login
        </Button>

        <Box display="flex" justifyContent="space-between">
          <Typography
            variant="body2"
            onClick={() => navigate('/register')}
            sx={{
              fontSize: '0.9rem',
              color: 'primary.main',
              cursor: 'pointer',
            }}
          >
            Register
          </Typography>
          <Typography
            variant="body2"
            onClick={() => navigate(AppRoutes.ForgotPasswordSetEmail)}
            sx={{
              fontSize: '0.9rem',
              color: 'primary.main',
              cursor: 'pointer',
            }}
          >
            Forgot password?
          </Typography>
        </Box>
      </Box>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        message={snackbarMessage}
        sx={{
          '& .MuiSnackbarContent-root': {
            backgroundColor: 'red',
            fontSize: '18px',
            padding: '16px',
          },
        }}
      />
    </Page>
  );
};
