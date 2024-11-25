import { FC, PropsWithChildren } from 'react';
import { Header } from './Header';
import { Box } from '@mui/material';
import { Footer } from './Footer.tsx';

export const Layout: FC<PropsWithChildren> = (props) => {
  return (
    <Box
      sx={{
        width: '100%',
        display: 'flex',
        flexDirection: 'column',
        overflow: 'hidden',
        minHeight: '100vh',
        m: 0,
        p: 0,
      }}
    >
      <Header />

      <Box
        component="main"
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          width: '100%',
          flexGrow: 1,
          p: 0,
          my: 0,
          bgcolor: 'background.default',
          overflowY: 'auto',
          overflowX: 'hidden',
        }}
      >
        {props.children}
      </Box>

      <Footer />
    </Box>
  );
};
