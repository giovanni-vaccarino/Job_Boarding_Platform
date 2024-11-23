﻿import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Matches } from './pages/Matches.tsx';
import { Profile } from './pages/Profile.tsx';
import { Login } from './pages/Login.tsx';
import { Register } from './pages/Register.tsx';
import { Activity } from './pages/Activity.tsx';
import { CompanyMatches } from './pages/CompanyMatches.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
  Activity: '/activity',
  Profile: '/profile',
  Login: '/login',
  Register: '/register',
  Internship: '/internship/:id',
  CompanyMatches: '/companymatches/:id',
});

export const useAppRouter = () => {
  return createBrowserRouter(
    [
      {
        path: AppRoutes.Home,
        element: <Home />,
      },
      {
        path: AppRoutes.Matches,
        element: <Matches />,
      },
      {
        path: AppRoutes.Activity,
        element: <Activity />,
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
      {
        path: AppRoutes.CompanyMatches,
        element: <CompanyMatches />,
      },
    ],
    {
      future: {
        v7_startTransition: true, // Flag not working -> had to suppress warning in main.tsx
        v7_skipActionErrorRevalidation: true,
        v7_partialHydration: true,
        v7_normalizeFormMethod: true,
        v7_fetcherPersist: true,
        v7_relativeSplatPath: true,
      } as never,
    }
  );
};
