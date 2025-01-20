import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, Snackbar, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';
import { LoginInput } from '../models/auth/login.ts';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { TypeProfile } from '../models/auth/register.ts';

export const Login = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const navigate = useNavigateWrapper();
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);
  const dispatch = useAppDispatch();

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
          />
        </Box>

        <Button
          fullWidth
          variant="contained"
          onClick={async () => {
            const loginInput: LoginInput = {
              email: email,
              password: password,
            };
            console.log(loginInput);

            try {
              const res = await authApi.login(loginInput);

              console.log('idstudente' + res.profileId);

              dispatch(appActions.auth.successLogin(res));
              dispatch(
                appActions.auth.setProfileType({ type: TypeProfile.Student })
              ); // TODO change this to the actual profile type
              dispatch(
                appActions.auth.setProfileId({ id: res.profileId.toString() })
              );
              console.log(res.profileId.toString());
              navigate(AppRoutes.Profile, {
                id: res.profileId.toString(),
              });
            } catch (error) {
              const errorMessage = error.message.split('\\r')[0];
              console.error(errorMessage);
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
      />
    </Page>
  );
};
