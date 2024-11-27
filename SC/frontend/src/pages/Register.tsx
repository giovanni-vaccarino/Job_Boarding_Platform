import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import {
  Box,
  Button,
  MenuItem,
  Select,
  TextField,
  Typography,
} from '@mui/material';
import { useState } from 'react';
import { RegisterInput, TypeProfile } from '../models/auth/register.ts';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { ServiceType } from '../core/ioc/service-type.ts';

export const Register = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const navigate = useNavigateWrapper();
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);
  const dispatch = useAppDispatch();

  const [profile, setProfile] = useState<TypeProfile>(TypeProfile.Student);

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
          >
            <MenuItem value="" disabled>
              Select Profile
            </MenuItem>
            <MenuItem value={TypeProfile.Company}>Company</MenuItem>
            <MenuItem value={TypeProfile.Student}>Student</MenuItem>
          </Select>
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
            const registrationInput: RegisterInput = {
              email: email,
              password: password,
              confirmPassword: confirmPassword,
              profile: profile,
            };

            console.log(email);
            const res = await authApi.register(registrationInput);

            console.log(res);

            dispatch(appActions.auth.successLogin(res));
            navigate(AppRoutes.Profile);
          }}
        >
          Register
        </Button>
      </Box>
    </Page>
  );
};
