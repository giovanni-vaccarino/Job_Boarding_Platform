import {
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../../router.tsx';
import {
  ApplicationInfo,
  ApplicationStatus,
} from '../../models/application/application.ts';
import { useAppSelector } from '../../core/store';

export interface ReceivedApplicationTableProps {
  applications: ApplicationInfo[];
}

export const ReceivedApplicationTable = (
  props: ReceivedApplicationTableProps
) => {
  const { applications = [] } = props;
  const navigate = useNavigateWrapper();
  const auth = useAppSelector((state) => state.auth);
  const companyId = auth.profileId;

  // Sort applications so "Ongoing" is at the top
  const sortedApplications = applications.sort((a, b) => {
    if (
      a.applicationStatus === ApplicationStatus.OnlineAssessment &&
      b.applicationStatus !== ApplicationStatus.OnlineAssessment
    )
      return -1;
    if (
      a.applicationStatus !== ApplicationStatus.OnlineAssessment &&
      b.applicationStatus === ApplicationStatus.OnlineAssessment
    )
      return 1;
    return 0;
  });

  return (
    <TableContainer
      component={Paper}
      sx={{
        padding: '1rem',
        border: '1px solid #e0e0e0',
        borderRadius: '8px',
        mb: '3rem',
      }}
    >
      <Table
        sx={{
          minWidth: '80%',
        }}
      >
        <TableHead>
          <TableRow>
            <TableCell
              align="left"
              sx={{ pl: '3rem', fontWeight: 'bold', fontSize: '1.35rem' }}
            >
              Name
            </TableCell>
            <TableCell
              align="center"
              sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}
            >
              State
            </TableCell>
            <TableCell
              align="center"
              sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}
            >
              Submission date
            </TableCell>
            <TableCell
              align="center"
              sx={{ fontWeight: 'bold', fontSize: '1.25rem' }}
            >
              Action
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {sortedApplications.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={4}
                align="center"
                sx={{
                  fontStyle: 'italic',
                  color: 'black',
                  fontSize: '1.2rem',
                  textAlign: 'center',
                  mt: '2rem',
                }}
              >
                There is no available application yet
              </TableCell>
            </TableRow>
          ) : (
            sortedApplications.map((row, index) => (
              <TableRow
                key={index}
                sx={{
                  borderTop:
                    row.applicationStatus !==
                      ApplicationStatus.OnlineAssessment &&
                    index > 0 &&
                    sortedApplications[index - 1].applicationStatus ===
                      ApplicationStatus.OnlineAssessment
                      ? '3px solid #e0e0e0' // Thicker line
                      : '1px solid #e0e0e0',
                }}
              >
                <TableCell align="left" sx={{ pl: '3rem' }}>
                  {row.student.name}
                </TableCell>
                <TableCell align="center">{row.applicationStatus}</TableCell>
                <TableCell align="center">
                  {row.submissionDate.toString()}
                </TableCell>
                <TableCell align="center">
                  <IconButton
                    color="primary"
                    aria-label="view details"
                    onClick={() =>
                      navigate(AppRoutes.ApplicantDetailPage, {
                        applicationId: row.id.toString(),
                        studentId: row.student.id.toString(),
                        companyId: companyId ? companyId.toString() : '',
                        applicationStatus: row.applicationStatus.toString(),
                        submissionDate: row.submissionDate.toString(),
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
