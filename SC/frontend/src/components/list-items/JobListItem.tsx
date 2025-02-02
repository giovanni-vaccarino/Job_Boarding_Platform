import { Box, Typography, Button } from '@mui/material';
import PlaceIcon from '@mui/icons-material/Place';
import TimelineIcon from '@mui/icons-material/Timeline';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';

export interface JobListItemProps {
  companyName: string;
  jobTitle: string;
  location: string;
  datePosted: Date;
  important?: boolean;
  id?: string;
}

export const JobListItem = (props: JobListItemProps) => {
  const navigate = useNavigateWrapper();

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
          {props.location != undefined && (
            <PlaceIcon sx={{ color: 'primary.main', mr: 0.5 }} />
          )}

          {props.location != undefined && (
            <Typography variant="body2">{props?.location}</Typography>
          )}

          {props.datePosted != undefined && (
            <TimelineIcon fontSize="small" sx={{ ml: 2, mr: 0.5 }} />
          )}

          {props.datePosted != undefined && (
            <Typography variant="body2">
              Posted {props.datePosted?.toLocaleDateString()}
            </Typography>
          )}
        </Box>
      </Box>

      <Button
        variant="contained"
        onClick={() => {
          navigate(AppRoutes.Job, { id: props.id ?? '' });
        }}
        sx={{
          backgroundColor: props.important ? 'red' : 'primary.main',
          color: 'white',
          textTransform: 'none',
          px: '2rem',
          py: '0.5rem',
          fontSize: '1.25rem',
          borderRadius: '8px',
        }}
        id={"viewDetailsJob_" + props.id}
      >
        View Details
      </Button>
    </Box>
  );
};
