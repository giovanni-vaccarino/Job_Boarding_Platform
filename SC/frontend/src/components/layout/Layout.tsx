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
          flexGrow: 1,
          p: 0,
          m: 0,
          bgcolor: 'background.default',
          overflowY: 'auto',
        }}
      >
        {props.children}
      </Box>

      <Footer />
    </Box>
  );
};
