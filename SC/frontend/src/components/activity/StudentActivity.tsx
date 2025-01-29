import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { JobsTable, JobsTableHeader } from '../tables/JobsTable.tsx';
import { Box, Typography } from '@mui/material';
import { ApplicationInfo } from '../../models/application/application.ts';

const mapApplicationToJobsTableHeader = (
  application: ApplicationInfo
): JobsTableHeader => {
  return {
    title: application.internship.title,
    company: application.companyName,
    state: application.applicationStatus,
    location: application.internship.location,
    submissionDate: application.submissionDate.toString(),
    id: application.id.toString(),
  };
};

export interface StudentActivityProps {
  applications: ApplicationInfo[];
}

export const StudentActivity = (props: StudentActivityProps) => {
  return (
    <>
      <TitleHeader title={'My Jobs List'} />
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
        {props.applications.length > 0 ? (
          <JobsTable
            jobs={props.applications.map(mapApplicationToJobsTableHeader)}
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
