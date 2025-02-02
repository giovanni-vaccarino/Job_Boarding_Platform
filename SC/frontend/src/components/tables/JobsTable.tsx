import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  IconButton,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../../router.tsx';
import { useAppSelector } from '../../core/store';
import { ApplicationStatus } from '../../models/application/application.ts';

export interface JobsTableHeader {
  title: string;
  company: string;
  state: ApplicationStatus;
  location: string;
  submissionDate: string;
  id: string;
}

export interface JobsTableProps {
  jobs: JobsTableHeader[];
}

export const JobsTable = (props: JobsTableProps) => {
  const { jobs = [] } = props;
  const navigate = useNavigateWrapper();
  const auth = useAppSelector((state) => state.auth);
  const studentId = auth.profileId;

  const getDatePart = (timestamp: string): string => {
    const date = new Date(timestamp);
    return date.toISOString().split('T')[0];
  };

  console.log('Application available:' + props);
  return (
    <TableContainer
      component={Paper}
      sx={{
        padding: '1rem',
        boxShadow: 'none',
        border: '1px solid #e0e0e0',
        borderRadius: '8px',
        mb: '3rem',
      }}
    >
      <Table
        sx={{
          minWidth: 650,
        }}
        aria-label="customized table"
      >
        <TableHead>
          <TableRow>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.35rem' }}>
              Title
            </TableCell>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}>
              Company
            </TableCell>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}>
              State
            </TableCell>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}>
              Location
            </TableCell>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}>
              Submission Date
            </TableCell>
            <TableCell sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}>
              Action
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {jobs.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={6}
                sx={{ textAlign: 'center', fontStyle: 'italic' }}
              >
                NO AVAILABLE JOB
              </TableCell>
            </TableRow>
          ) : (
            jobs.map((row, index) => (
              <TableRow key={index}>
                <TableCell>{row.title}</TableCell>
                <TableCell>{row.company}</TableCell>
                <TableCell>{row.state}</TableCell>
                <TableCell>{row.location}</TableCell>
                <TableCell>{getDatePart(row.submissionDate)}</TableCell>
                <TableCell>
                  <IconButton
                    color="primary"
                    aria-label="view details"
                    onClick={() =>
                      navigate(AppRoutes.Application, {
                        studentId: (studentId ?? '').toString(),
                        applicationId: row.id,
                      })
                    }
                  >
                    <VisibilityIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))
          )}
        </TableBody>
      </Table>
    </TableContainer>
  );
};
