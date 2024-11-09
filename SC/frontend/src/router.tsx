import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Matches } from './pages/Matches.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
});

export const useAppRouter = () => {
  return createBrowserRouter([
    {
      path: AppRoutes.Home,
      element: <Home />,
    },
    {
      path: AppRoutes.Matches,
      element: <Matches />,
    },
  ]);
};
