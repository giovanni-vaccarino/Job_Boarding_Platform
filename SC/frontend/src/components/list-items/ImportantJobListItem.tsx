import { Box, Typography, Button } from '@mui/material';
import PlaceIcon from '@mui/icons-material/Place';
import TimelineIcon from '@mui/icons-material/Timeline';

export interface ImportantJobListItemProps {
  companyName: string;
  jobTitle: string;
  location: string;
  datePosted: string;
}

export const ImportantJobListItem = (props: ImportantJobListItemProps) => {
  return (
    <Box
      sx={{
        width: '73.5%',
        height: '4.2rem',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        px: '5rem',
        py: '1.5rem',
        border: '1px solid #e0e0e0',
        borderRadius: '10px',
        backgroundColor: '#ffffff',
        boxShadow: '0px 2px 8px rgba(0, 0, 0, 0.1)',
      }}
    >
      <Box>
        <Typography color="text.secondary">{props.companyName}</Typography>
        <Typography
          color="text.primary"
          sx={{ mt: '0.25rem', fontSize: '1.25rem', fontWeight: 'bold' }}
        >
          {props.jobTitle}
        </Typography>

        <Box display="flex" alignItems="center" mt={0.5}>
          <PlaceIcon sx={{ color: 'primary.main', mr: 0.5 }} />
          <Typography variant="body2">{props.location}</Typography>

          <TimelineIcon fontSize="small" sx={{ ml: 2, mr: 0.5 }} />
          <Typography variant="body2">Posted {props.datePosted}</Typography>
        </Box>
      </Box>

      <Button
        variant="contained"
        sx={{
          backgroundColor: 'red',
          color: 'white',
          textTransform: 'none',
          px: '2rem',
          py: '0.5rem',
          fontSize: '1.25rem',
          borderRadius: '8px',
        }}
      >
        View Details
      </Button>
    </Box>
  );
};
