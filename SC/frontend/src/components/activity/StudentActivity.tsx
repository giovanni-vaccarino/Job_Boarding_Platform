import { TitleHeader } from '../page-headers/TitleHeader.tsx';
import { JobsTable, JobsTableHeader } from '../tables/JobsTable.tsx';
import { Box, Typography } from '@mui/material';
import { Application } from '../../models/application/application.ts';

const mapApplicationToJobsTableHeader = (
  application: Application
): JobsTableHeader => {
  return {
    title: application.internship.title,
    company: 'Name_Company',
    state: application.applicationStatus.toString(),
    location: application.internship.location,
    submissionDate: application.submissionDate.toDateString(),
  };
};

const exampleData = [
  {
    title: 'Software Engineer',
    company: 'Tech Solutions Inc.',
    state: 'California',
    location: 'San Francisco',
    submissionDate: '2024-11-25',
  },
  {
    title: 'Data Analyst',
    company: 'Data Insights LLC',
    state: 'New York',
    location: 'New York City',
    submissionDate: '2024-11-20',
  },
  {
    title: 'Product Manager',
    company: 'Innovatech Corp.',
    state: 'Texas',
    location: 'Austin',
    submissionDate: '2024-11-15',
  },
];

export interface StudentActivityProps {
  applications: Application[];
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
