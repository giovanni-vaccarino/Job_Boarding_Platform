import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { Box, Typography } from '@mui/material';
import {
  CompanyJobsTable,
  CompanyJobsTableHeader,
} from '../tables/CompanyJobsTable.tsx';
import { Internship } from '../../models/internship/internship.ts';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';

const mapInternshipToCompanyTableHeader = (
  internship: Internship
): CompanyJobsTableHeader => {
  return {
    title: internship.title,
    applications: internship.numberOfApplications,
    jobType: internship.jobType.toString(),
    location: internship.location,
  };
};

export interface CompanyActivityProps {
  internship: Internship[];
}

export const CompanyActivity = (props: CompanyActivityProps) => {
  const navigate = useNavigateWrapper();

  console.log(props.internship);

  return (
    <>
      <TitleHeader title={'Jobs List'} />
      <Box sx={{ display: 'flex', justifyContent: 'right', width: '80%' }}>
        <Typography
          sx={{
            color: 'primary.main',
            textDecoration: 'underline',
            fontStyle: 'italic',
            mt: '2rem',
          }}
          onClick={() => {
            navigate(AppRoutes.NewJob);
          }}
        >
          Add New Job
        </Typography>
      </Box>

      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          width: '90%',
          margin: '0 auto',
          marginTop: '1rem',
        }}
      >
        {props.internship.length > 0 ? (
          <CompanyJobsTable
            jobs={props.internship.map(mapInternshipToCompanyTableHeader)}
          />
        ) : (
          <Typography sx={{ fontStyle: 'italic' }}>
            THERE IS NO JOB AVAILABLE
          </Typography>
        )}
      </Box>
    </>
  );
};
