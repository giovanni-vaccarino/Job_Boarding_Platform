import { Box, Typography } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';

export const Home = () => {
  return (
    <Page>
      <Box
        sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}
      >
        <Typography>Home</Typography>
      </Box>
    </Page>
  );
};
