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



export interface StudentsTableHeader {
  name: string;
  suggestedJob: string;
}

export interface StudentsTableProps {
  students: StudentsTableHeader[];
}

export const StudentsTable = (props: StudentsTableProps) => {
  const { students = [] } = props;
  const navigate = useNavigateWrapper();

  return (
    <TableContainer
      component={Paper}
      sx={{
        padding: '1rem',
        boxShadow: 'none',
        border: '1px solid #e0e0e0',
        borderRadius: '8px',
        mb: '3rem',
        display: 'flex',
        justifyContent: 'center',
      }}
    >
      <Table
        sx={{
          minWidth: 650,
          margin: '0 auto',
        }}
        aria-label="customized table"
      >
        <TableHead>
          <TableRow>
            <TableCell
              sx={{
                paddingLeft: '3%',
                fontWeight: 'bold',
                fontSize: '1.35rem',
                width: '30%',
              }}
            >
              Name
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                textAlign: 'center',
                width: '40%',
                paddingLeft: '20%',
              }}
            >
              Suggested Job
            </TableCell>
            <TableCell
              sx={{
                fontWeight: 'bold',
                fontSize: '1.25rem',
                textAlign: 'right',
                width: '30%',
                paddingRight: '10%',
              }}
            >
              Action
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {students.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={3}
                sx={{ textAlign: 'center', fontStyle: 'italic' }}
              >
                NO DATA
              </TableCell>
            </TableRow>
          ) : (
            students.map((row, index) => (
              <TableRow key={index}>
                <TableCell
                  sx={{
                    width: '30%',
                    paddingLeft: '3%',
                  }}
                >
                  {row.name}
                </TableCell>
                <TableCell
                  sx={{
                    textAlign: 'center',
                    width: '60%',
                    paddingLeft: '20%',
                  }}
                >
                  {row.suggestedJob}
                </TableCell>
                <TableCell
                  sx={{
                    textAlign: 'right',
                    width: '10%',
                    paddingRight: '4.7%',
                  }}
                >
                  <IconButton
                    color="primary"
                    aria-label="view details"
                    onClick={() => {
                      navigate(AppRoutes.ApplicantDetailPage);
                    }}
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
