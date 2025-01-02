import { Box, Typography, Button } from '@mui/material';
import PlaceIcon from '@mui/icons-material/Place';
import TimelineIcon from '@mui/icons-material/Timeline';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { ApplyToInternshipInput } from '../../models/internship/internship.ts';
import { useService } from '../../core/ioc/ioc-provider.tsx';
import { IInternshipApi } from '../../core/API/internship/IInternshipApi.ts';
import { ServiceType } from '../../core/ioc/service-type.ts';

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
  const apiInternship = useService<IInternshipApi>(ServiceType.InternshipApi);

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
          <Typography variant="body2">
            Posted {props.datePosted.toLocaleDateString()}
          </Typography>
        </Box>
      </Box>

      <Button
        variant="contained"
        onClick={async () => {
          const applyToInternshipInput: ApplyToInternshipInput = {
            internshipId: props.id,
            studentId: '1',
          };

          const res = await apiInternship.postApplyToInternship(
            applyToInternshipInput
          );
          console.log(res);
          navigate(AppRoutes.Job, { id: props.id ?? '' });
        }}
        //onClick={() => navigate(AppRoutes.Job)}
        sx={{
          backgroundColor: props.important ? 'red' : 'primary.main',
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
