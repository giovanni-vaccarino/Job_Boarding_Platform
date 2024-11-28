import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Profile } from './pages/Profile.tsx';
import { Login } from './pages/Login.tsx';
import { Register } from './pages/Register.tsx';
import { Application } from './pages/Application.tsx';
import { ConfirmPage } from './pages/ConfirmPage.tsx';
import { NewJob } from './pages/NewJob.tsx';
import { OnlineAssessment } from './pages/OnlineAssessment.tsx';
import { ApplicantDetailPage } from './pages/ApplicantDetailPage.tsx';
import { NewJobQuestion } from './pages/NewJobQuestion.tsx';
import { ReceivedApplication } from './pages/ReceivedApplications.tsx';
import { Activity } from './pages/Activity.tsx';
import { Matches } from './pages/Matches.tsx';
import { ForgotPasswordSetEmail } from './pages/ForgotPasswordSetEmail.tsx';
import { ForgotPasswordSetPassword } from './pages/ForgotPasswordSetPassword.tsx';
import { Job } from './pages/Job.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
  Application: '/application',
  Job: '/job',
  ConfirmPage: '/confirm-page',
  Activity: '/activity',
  Profile: '/profile',
  Login: '/login',
  ForgotPasswordSetPassword: '/forgot-password-set-password',
  ForgotPasswordSetEmail: '/forgot-password-set-email',
  Register: '/register',
  Internship: '/internship/:id',
  ReceivedApplication: '/received-application',
  OnlineAssessment: '/online-assessment',
  NewJobQuestion: '/new-job-question',
  NewJob: '/new-job',
  ApplicantDetailPage: '/applicant-detail-page',
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
        path: AppRoutes.Application,
        element: <Application />,
      },
      {
        path: AppRoutes.Job,
        element: <Job />,
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
        path: AppRoutes.ForgotPasswordSetEmail,
        element: <ForgotPasswordSetEmail />,
      },
      {
        path: AppRoutes.ForgotPasswordSetPassword,
        element: <ForgotPasswordSetPassword />,
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
        path: AppRoutes.NewJob,
        element: <NewJob />,
      },
      {
        path: AppRoutes.NewJobQuestion,
        element: <NewJobQuestion />,
      },
      {
        path: AppRoutes.ApplicantDetailPage,
        element: <ApplicantDetailPage nameApplicant={'mockname'} />,
      },
      {
        path: AppRoutes.ReceivedApplication,
        element: <ReceivedApplication />,
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
