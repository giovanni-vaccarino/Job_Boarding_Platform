import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Matches } from './pages/Matches.tsx';
import { Profile } from './pages/Profile.tsx';
import { Login } from './pages/Login.tsx';
import { Register } from './pages/Register.tsx';
import { JobDescription } from './pages/JobDescription.tsx';
import { ConfirmPage } from './pages/ConfirmPage.tsx';
import { Activity } from './pages/Activity.tsx';
import { NewJob } from './pages/NewJob.tsx';
import { CompanyMatches } from './pages/CompanyMatches.tsx';
import { CompanyActivity } from './pages/CompanyActivity.tsx';
import { OnlineAssessment } from './pages/OnlineAssessment.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
  JobDescription: '/job-description',
  ConfirmPage: '/confirm-page',
  Activity: '/activity',
  CompanyActivity: '/companyactivity',
  Profile: '/profile',
  Login: '/login',
  Register: '/register',
  Internship: '/internship/:id',
  OnlineAssessment: '/online-assessment',
  CompanyMatches: '/companymatches/:id',
  NewJob: '/newjob',
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
        path: AppRoutes.JobDescription,
        element: <JobDescription />,
      },
      {
        path: AppRoutes.ConfirmPage,
        element: <ConfirmPage />,
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
        path: AppRoutes.OnlineAssessment,
        element: <OnlineAssessment />,
      },
      {
        path: AppRoutes.CompanyMatches,
        element: <CompanyMatches />,
      },
      {
        path: AppRoutes.NewJob,
        element: <NewJob />,
      },
      {
        path: AppRoutes.CompanyActivity,
        element: <CompanyActivity />,
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
