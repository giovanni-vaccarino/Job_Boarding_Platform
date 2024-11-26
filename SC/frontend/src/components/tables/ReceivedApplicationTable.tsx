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

export interface ReceivedApplicationTableHeader {
  name: string;
  state: string;
  submissionDate: string;
}

export interface ReceivedApplicationTableProps {
  applications: ReceivedApplicationTableHeader[];
}

export const ReceivedApplicationTable = (
  props: ReceivedApplicationTableProps
) => {
  const { applications = [] } = props;

  // Sort applications so "Ongoing" is at the top
  const sortedApplications = applications.sort((a, b) => {
    if (a.state === 'Ongoing' && b.state !== 'Ongoing') return -1;
    if (a.state !== 'Ongoing' && b.state === 'Ongoing') return 1;
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
                sx={{ textAlign: 'center', fontStyle: 'italic' }}
              >
                NO DATA
              </TableCell>
            </TableRow>
          ) : (
            sortedApplications.map((row, index) => (
              <TableRow
                key={index}
                sx={{
                  borderTop:
                    row.state !== 'Ongoing' &&
                    index > 0 &&
                    sortedApplications[index - 1].state === 'Ongoing'
                      ? '3px solid #e0e0e0' // Thicker line
                      : '1px solid #e0e0e0',
                }}
              >
                <TableCell align="left" sx={{ pl: '3rem' }}>
                  {row.name}
                </TableCell>
                <TableCell align="center">{row.state}</TableCell>
                <TableCell align="center">{row.submissionDate}</TableCell>
                <TableCell align="center">
                  <IconButton color="primary" aria-label="view details">
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
