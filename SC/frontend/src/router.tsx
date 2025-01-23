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
import { InternshipDetailsLoader } from './core/API/loader/InternshipDetailsLoader.ts';
import { useService } from './core/ioc/ioc-provider.tsx';
import { ServiceType } from './core/ioc/service-type.ts';
import { IInternshipApi } from './core/API/internship/IInternshipApi.ts';
import { IStudentApi } from './core/API/student/IStudentApi.ts';
import { ApplicationLoader } from './core/API/loader/ApplicationLoader.ts';
import { useAppSelector } from './core/store';
import { TypeProfile } from './models/auth/register.ts';
import { InternshipCompanyLoader } from './core/API/loader/InternshipCompanyLoader.ts';
import { StudentLoader } from './core/API/loader/StudentLoader.ts';
import { CompanyLoader } from './core/API/loader/CompanyLoader.ts';
import { ICompanyApi } from './core/API/company/ICompanyApi.ts';
import { MatchesLoaderStudent } from './core/API/loader/MatchesLoaderStudents.ts';
import { MatchesLoaderCompany } from './core/API/loader/MatchesLoaderCompany.ts';
import { IMatchApi } from './core/API/match/IMatchApi.ts';
import { ApplicantDetailsLoader } from './core/API/loader/ApplicantDetailsLoader.ts';
import { InternshipLoader } from './core/API/loader/InternshipLoader.ts';
import { ApplicationDetailsLoader } from './core/API/loader/ApplicationDetailsLoader.ts';
import { QuestionLoader } from './core/API/loader/QuestionLoader.ts';
import { ApplicationsPerInternship } from './core/API/loader/ApplicationsPerInternship.ts';

export const AppRoutes = Object.freeze({
  Home: '/',
  Matches: '/matches/:id',
  Application: '/application/:studentId/:applicationId',
  Job: '/job/:id',
  ConfirmPage: '/confirm-page',
  Activity: '/activity/:id',
  Profile: `/profile/:id`,
  Login: '/login',
  ForgotPasswordSetPassword: '/forgot-password-set-password',
  ForgotPasswordSetEmail: '/forgot-password-set-email',
  Register: '/register',
  Internship: '/internship/:id',
  ReceivedApplications: '/received-application/:internshipId/:companyId',
  OnlineAssessment: '/online-assessment/:internshipId/:applicationId',
  NewJobQuestion: '/new-job-question',
  NewJob: '/new-job',
  ApplicantDetailPage: '/applicant-detail-page/:id',
});

export const useAppRouter = () => {
  const internshipApi = useService<IInternshipApi>(ServiceType.InternshipApi);
  const studentApi = useService<IStudentApi>(ServiceType.StudentApi);
  const companyApi = useService<ICompanyApi>(ServiceType.CompanyApi);
  const authState = useAppSelector((state) => state.auth);
  const profileType = authState.profileType;
  const matchApi = useService<IMatchApi>(ServiceType.MatchApi);

  return createBrowserRouter(
    [
      {
        path: AppRoutes.Home,
        loader: () => InternshipLoader(internshipApi),
        element: <Home />,
      },
      {
        path: AppRoutes.Matches,
        loader: ({ params }) =>
          profileType === TypeProfile.Student
            ? MatchesLoaderStudent(matchApi, params.id || '')
            : MatchesLoaderCompany(matchApi, params.id || ''),
        element: <Matches />,
      },
      {
        //Note: in the activity page the users see the applications, while the companies see their internships
        path: AppRoutes.Activity,
        loader: ({ params }) =>
          profileType === TypeProfile.Student
            ? ApplicationLoader(studentApi, params.id || '')
            : InternshipCompanyLoader(internshipApi, params.id || ''),
        element: <Activity />,
      },

      {
        path: AppRoutes.Application,
        loader: ({ params }) =>
          ApplicationDetailsLoader(
            studentApi,
            params.studentId || '',
            params.applicationId || ''
          ),
        element: <Application />,
      },
      {
        path: AppRoutes.Job,
        loader: ({ params }) =>
          InternshipDetailsLoader(internshipApi, params.id || ''),
        element: <Job />,
      },
      {
        path: AppRoutes.ConfirmPage,
        element: <ConfirmPage />,
      },
      {
        path: AppRoutes.Profile,
        loader: ({ params }) =>
          profileType === TypeProfile.Student
            ? StudentLoader(studentApi, params.id || '')
            : CompanyLoader(companyApi, params.id || ''),
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
        loader: ({ params }) =>
          QuestionLoader(internshipApi, params.internshipId || ''),
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
        loader: ({ params }) =>
          ApplicantDetailsLoader(matchApi, params.id || ''),
        element: <ApplicantDetailPage/>,
      },
      {
        path: AppRoutes.ReceivedApplications,
        loader: ({ params }) =>
          ApplicationsPerInternship(
            internshipApi,
            params.internshipId || '',
            params.companyId || ''
          ),
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
