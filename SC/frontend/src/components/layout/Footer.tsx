import { useState } from 'react';
import { Box, Button, Typography } from '@mui/material';
import { CreateFeedback } from '../job-description/CreateFeedback.tsx';
import { useAppSelector } from '../../core/store';

export const Footer = () => {
  const [showFeedback, setShowFeedback] = useState(false);
  const auth = useAppSelector((state) => state.auth);
  const logged = auth.loggedIn;

  const handleToggleFeedback = () => {
    setShowFeedback((prev) => !prev);
  };

  return (
    <Box
      component="footer"
      sx={{
        m: 0,
        p: 2,
        width: '100%',
        textAlign: 'center',
        bgcolor: 'primary.main',
        color: 'common.white',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <Typography
        variant="body2"
        sx={{
          fontWeight: 'bold',
          textTransform: 'uppercase',
          letterSpacing: '0.1em',
        }}
      >
        ©2024 SC Platform. All Rights Reserved.
      </Typography>
      <Box
        sx={{
          display: 'flex',
          alignSelf: 'flex-end',
          marginRight: '1rem',
        }}
      >
        <Button onClick={handleToggleFeedback}>
          <Typography
            variant="body2"
            sx={{
              color: 'common.white',
              fontWeight: 'bold',
              fontSize: '0.98rem',
            }}
          >
            {!showFeedback ? 'Leave a feedback for us' : 'Close feedback'}
          </Typography>
        </Button>
      </Box>
      {showFeedback && logged && (
        <Box
          sx={{
            bgcolor: 'secondary.main',
            p: 2,
            mt: 2,
            borderRadius: 1,
            width: '100%',
            maxWidth: '600px',
            margin: 'auto',
            color: 'common.black',
          }}
        >
          <CreateFeedback applicationId={-1} />
        </Box>
      )}
    </Box>
  );
};
