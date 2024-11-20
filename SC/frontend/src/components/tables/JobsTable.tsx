import React from 'react';
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

export const JobsTable = ({ data }) => {
  return (
    <TableContainer
      component={Paper}
      sx={{ boxShadow: 'none', borderRadius: '8px' }}
    >
      <Table sx={{ minWidth: 650 }} aria-label="customized table">
        <TableHead>
          <TableRow>
            <TableCell sx={{ fontWeight: 'bold' }}>Title</TableCell>
            <TableCell sx={{ fontWeight: 'bold' }}>Company</TableCell>
            <TableCell sx={{ fontWeight: 'bold' }}>State</TableCell>
            <TableCell sx={{ fontWeight: 'bold' }}>Location</TableCell>
            <TableCell sx={{ fontWeight: 'bold' }}>Submission Date</TableCell>
            <TableCell sx={{ fontWeight: 'bold' }}>Action</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row, index) => (
            <TableRow key={index}>
              <TableCell>{row.title}</TableCell>
              <TableCell>{row.company}</TableCell>
              <TableCell>{row.state}</TableCell>
              <TableCell>{row.location}</TableCell>
              <TableCell>{row.submissionDate}</TableCell>
              <TableCell>
                <IconButton color="primary" aria-label="view details">
                  <VisibilityIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};
