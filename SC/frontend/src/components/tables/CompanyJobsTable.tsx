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
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../../router.tsx';
import { useAppSelector } from '../../core/store';

export interface CompanyJobsTableHeader {
  title: string;
  applications: number; // Number of applications received
  jobType: string; // Job Type (e.g., Full Time)
  location: string;
  internshipId: string;
}

export interface CompanyJobsTableProps {
  jobs: CompanyJobsTableHeader[];
}

export const CompanyJobsTable = (props: CompanyJobsTableProps) => {
  const { jobs = [] } = props;
  const navigate = useNavigateWrapper();
  const auth = useAppSelector((state) => state.auth);
  const profileId = auth.profileId;

  console.log(jobs);
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
            <TableCell
              sx={{ fontWeight: 'bold', fontSize: '1.35rem', width: '10%' }}
            >
              Title
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                textAlign: 'center',
                width: '40%',
              }}
            >
              Application received
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                textAlign: 'center',
                width: '16%',
              }}
            >
              Job Type
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                textAlign: 'center',
                width: '16%',
              }}
            >
              Location
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                width: '16%',
                pl: '4rem',
              }}
            >
              Action
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {jobs.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={5}
                sx={{ textAlign: 'center', fontStyle: 'italic' }}
              >
                NO DATA
              </TableCell>
            </TableRow>
          ) : (
            jobs.map((row, index) => (
              <TableRow key={index}>
                <TableCell sx={{ width: '10%' }}>{row.title}</TableCell>
                <TableCell sx={{ textAlign: 'center', width: '40%' }}>
                  {row.applications}
                </TableCell>
                <TableCell sx={{ textAlign: 'center', width: '16%' }}>
                  {row.jobType}
                </TableCell>
                <TableCell sx={{ textAlign: 'center', width: '16%' }}>
                  {row.location}
                </TableCell>
                <TableCell
                  sx={{ textAlign: 'center', width: '16%', pr: '2rem' }}
                >
                  <IconButton
                    color="primary"
                    aria-label="view details"
                    onClick={() =>
                      navigate(AppRoutes.ReceivedApplications, {
                        internshipId: row.internshipId,
                        companyId: profileId ? profileId.toString() : '',
                      })
                    }
                  >
                    <VisibilityIcon />
                  </IconButton>
                  <IconButton color="error" aria-label="delete">
                    <DeleteOutlineIcon />
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
