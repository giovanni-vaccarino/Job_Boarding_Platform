import { RouterProvider } from 'react-router-dom';
import { useAppRouter } from './router';
import { GlobalStyles, ThemeProvider } from '@mui/material';
import { theme } from './theme';

function App() {
  const router = useAppRouter();

  return (
    <ThemeProvider theme={theme}>
      <GlobalStyles
        styles={{
          body: {
            margin: 0,
          },
        }}
      />
      <RouterProvider router={router} />
    </ThemeProvider>
  );
}

export default App;
