import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Matches } from './pages/Matches.tsx';
import { Profile } from './pages/Profile.tsx';
import { Login } from './pages/Login.tsx';
import { Register } from './pages/Register.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
  Profile: '/profile',
  Login: '/login',
  Register: '/register',
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
    {
      path: AppRoutes.Profile,
      element: <Profile />,
    },
    {
      path: AppRoutes.Login,
      element: <Login />,
    },
    {
      path: AppRoutes.Register,
      element: <Register />,
    },
  ]);
};
