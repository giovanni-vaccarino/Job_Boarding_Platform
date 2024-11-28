import { createBrowserRouter } from 'react-router-dom';
import { Home } from './pages/Home.tsx';
import { Profile } from './pages/Profile.tsx';
import { Login } from './pages/Login.tsx';
import { Register } from './pages/Register.tsx';
import { JobDescription } from './pages/JobDescription.tsx';
import { ConfirmPage } from './pages/ConfirmPage.tsx';
import { NewJob } from './pages/NewJob.tsx';
import { OnlineAssessment } from './pages/OnlineAssessment.tsx';
import { ApplicantDetailPage } from './pages/ApplicantDetailPage.tsx';
import { NewJobQuestion } from './pages/NewJobQuestion.tsx';
import { CompanyJobDescription } from './pages/CompanyJobDescription.tsx';
import { ReceivedApplication } from './pages/ReceivedApplications.tsx';
import { Activity } from './pages/Activity.tsx';
import { Matches } from './pages/Matches.tsx';
import { ForgotPasswordSetEmail } from './pages/ForgotPasswordSetEmail.tsx';
import { ForgotPasswordSetPassword } from './pages/ForgotPasswordSetPassword.tsx';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches',
  JobDescription: '/job-description',
  CompanyJobDescription: '/company-job-description',
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
        path: AppRoutes.JobDescription,
        element: <JobDescription />,
      },
      {
        path: AppRoutes.CompanyJobDescription,
        element: <CompanyJobDescription />,
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
