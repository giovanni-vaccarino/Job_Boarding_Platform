import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { appActions, useAppDispatch } from '../core/store';
import { AppRoutes } from '../router.tsx';

export const ForgotPasswordSetPassword = () => {
  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

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
          onClick={() => {
            dispatch(
              appActions.global.setConfirmMessage({
                newMessage: 'Password Changed Successfully',
              })
            );
            navigate(AppRoutes.ConfirmPage);
          }}
        >
          Change Password
        </Button>
      </Box>
    </Page>
  );
};
