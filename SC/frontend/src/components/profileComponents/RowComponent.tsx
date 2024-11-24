import { Box, Button, Typography } from '@mui/material';
import ModeIcon from '@mui/icons-material/Mode';
import RemoveRedEyeIcon from '@mui/icons-material/RemoveRedEye';

interface RowComponentProps {
  label: string;
  value: string;
  buttons: string[];
}

export const RowComponent: React.FC<RowComponentProps> = ({ label, value, buttons}) => {
  return (
    <Box sx = {{
      display: 'flex',
      flexDirection: 'column',
      marginLeft: '30%',
    }}>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row',
          justifyContent: 'space-between',
          flex: '1',
        }}
      >
        <Typography sx={{ fontSize: '1.3rem', fontWeight: '500'}}>
          {label}
        </Typography>
        {buttons.includes('edit') &&
        <Button variant="text" sx = {{marginRight: '20%'}} startIcon={<ModeIcon />}>
        </Button>
        }
        {buttons.includes('view') &&
          <Button variant="text" sx = {{marginRight: '48%'}} startIcon={<RemoveRedEyeIcon />}></Button>
        }
      </Box>
      <Box sx={{ marginTop: '1%' }}>
        <Typography
          sx={{
            fontSize: '1.25rem',
            fontWeight: '500',
            color: 'rgba(0, 0, 0, 0.4)', // Black with 70% opacity
          }}
        >
          {value}
        </Typography>
      </Box>
    </Box>
  );
};