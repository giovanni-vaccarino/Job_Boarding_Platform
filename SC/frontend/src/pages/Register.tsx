import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Box, Button, TextField, Typography } from '@mui/material';
import { useState } from 'react';

export const Register = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');

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
        >
          Register
        </Button>
      </Box>
    </Page>
  );
};
