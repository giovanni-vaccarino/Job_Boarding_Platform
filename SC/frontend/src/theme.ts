import { createTheme } from '@mui/material';

export const theme = createTheme({
  palette: {
    primary: {
      main: '#338573',
    },
    secondary: {
      main: '#f6f8fa',
    },
    common: {
      black: '#3c4257',
    },
    background: {
      default: '#fff',
    },
    success: {
      main: '#00c853',
      light: '#00e676',
    },
  },
  spacing: 8,
  typography: {
    fontFamily: '"Noto Sans Hebrew", sans-serif',
  },
  components: {
    MuiBackdrop: {
      styleOverrides: {
        root: {
          backgroundColor: 'rgba(99, 90, 250, 0.24)',
        },
      },
    },
    MuiButtonBase: {
      defaultProps: {
        disableTouchRipple: true,
        disableRipple: true,
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'unset',
        },
      },
      defaultProps: {
        disableElevation: true,
      },
    },
  },
});
