import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import {
  Box,
  MenuItem,
  Select,
  Snackbar,
  TextField,
  Typography,
} from '@mui/material';
import { useEffect, useState } from 'react';
import { RegisterInput, TypeProfile } from '../models/auth/register.ts';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';
import { BusyButton } from '../components/button/BusyButton.tsx';

export const Register = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [emailError, setEmailError] = useState<string>('');
  const navigate = useNavigateWrapper();
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);
  const dispatch = useAppDispatch();
  const [isDisabled, setIsDisabled] = useState(true);
  const [isLoading, setIsLoading] = useState(false);

  const [profile, setProfile] = useState<TypeProfile>(TypeProfile.Student);

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

    const validateEmail = (email: string) => {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    };

  useEffect(() => {
    if (
      email.length > 0 &&
      password.length > 0 &&
      confirmPassword.length > 0
    ) {
      setIsDisabled(false);
    } else {
      setIsDisabled(true);
    }
  }, [email, password, confirmPassword, profile]);

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setEmail(value);
    if (!validateEmail(value)) {
      setEmailError('Invalid email format');
    } else {
      setEmailError('');
    }
  };

  return (
    <Page>
      <TitleHeader title={'Register'} />

      <Box
        sx={{
          width: '100%',
          maxWidth: '500px',
          margin: 'auto',
          padding: 3,
          mt: '3rem',
          mb: '3rem',
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
            Email <span style={{ color: 'red' }}>*</span>
          </Typography>
          <TextField
            value={email}
            onChange={handleEmailChange}
            fullWidth
            variant="outlined"
            placeholder="Email"
            required
            margin="normal"
            id="emailField"
            error={!!emailError}
            helperText={emailError}
          />
        </Box>

        <Box
          sx={{
            mb: '1rem',
          }}
        >
          <Typography sx={{ fontSize: '1.35rem', fontWeight: '500' }}>
            Password <span style={{ color: 'red' }}>*</span>
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
            id="passwordField"
          />
        </Box>

        <Box
          sx={{
            mb: '1.5rem',
          }}
        >
          <Typography sx={{ fontSize: '1.35rem', fontWeight: '500' }}>
            Confirm Password <span style={{ color: 'red' }}>*</span>
          </Typography>
          <TextField
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            fullWidth
            variant="outlined"
            placeholder="Confirm Password"
            type="password"
            required
            margin="normal"
            id="confirmPasswordField"
          />
        </Box>

        <Box
          sx={{
            mb: '1.5rem',
          }}
        >
          <Typography sx={{ fontSize: '1.35rem', fontWeight: '500' }}>
            Profile <span style={{ color: 'red' }}>*</span>
          </Typography>
          <Select
            value={profile}
            onChange={(e) => setProfile(e.target.value as TypeProfile)}
            fullWidth
            variant="outlined"
            displayEmpty
            required
            sx={{ marginTop: '0.5rem' }}
            id="profileField"
          >
            <MenuItem value="" disabled>
              Select Profile
            </MenuItem>
            <MenuItem id="Company" value={TypeProfile.Company}>
              Company
            </MenuItem>
            <MenuItem id="Student" value={TypeProfile.Student}>
              Student
            </MenuItem>
          </Select>
        </Box>

        <BusyButton
          disabled={isDisabled}
          isBusy={isLoading}
          fullWidth
          variant="contained"
          id="registerButton"
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
            const registrationInput: RegisterInput = {
              email: email,
              password: password,
              confirmPassword: confirmPassword,
              profileType: profile,
            };

            try {
              setIsLoading(true);
              const res = await authApi.register(registrationInput);

              // @ts-ignore
              dispatch(appActions.auth.successLogin(res));
              dispatch(appActions.auth.setProfileType({ type: profile }));
              navigate(AppRoutes.Profile, {
                id: res.profileId.toString(),
              });
              setInterval(function () {
                window.location.reload();
              }, 500);
            } catch (error) {
              setIsLoading(false);
              // @ts-ignore
              const errorMessage = error.message.split('\\r')[0];

              console.error(
                'Full error object:',
                JSON.stringify(error, null, 2)
              );

              setSnackbarMessage(errorMessage);
              setSnackbarOpen(true);
            }
          }}
        >
          Register
        </BusyButton>
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
