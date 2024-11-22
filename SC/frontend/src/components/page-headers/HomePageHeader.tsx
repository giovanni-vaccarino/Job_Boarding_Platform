import { Box, Button, Stack, TextField, Typography } from '@mui/material';
import home_background from '../../assets/home_background.png';

export const HomePageHeader = () => {
  return (
    <Box
      sx={{
        overflow: 'hidden',
        width: '100%',
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        textAlign: 'center',
        padding: '0rem 3rem',
        px: '10rem',
        paddingTop: '1rem',
        backgroundColor: '#f5f7f6',
      }}
    >
      <Stack direction="column" spacing={2} width="50%">
        <Typography
          sx={{
            fontSize: '4rem',
            textAlign: 'left',
            maxWidth: '60%',
            fontWeight: 'bolder',
            lineHeight: 1.15,
            marginBottom: '2rem',
          }}
        >
          Find A <span style={{ color: '#338573' }}>Job</span> That{' '}
          <span style={{ color: '#338573' }}>Matches</span> Your Passion
        </Typography>
        <Typography
          variant="subtitle1"
          sx={{
            textAlign: 'left',
            maxWidth: '60%',
            marginBottom: '30px',
            color: '#555',
          }}
        >
          Hand-picked opportunities to work from home, remotely, freelance,
          full-time, part-time, contract and internships.
        </Typography>

        {/* Search Input Section */}
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            maxWidth: '500px',
            width: '100%',
            marginBottom: '40px',
          }}
        >
          <TextField
            variant="outlined"
            placeholder="Search by job title..."
            fullWidth
            sx={{ backgroundColor: '#ffffff', borderRadius: '4px' }}
          />
          <Button
            variant="contained"
            sx={{
              marginLeft: '10px',
              backgroundColor: 'primary.main',
              color: '#fff',
            }}
          >
            Search
          </Button>
        </Box>
      </Stack>

      <Box sx={{ width: '50%', display: 'flex', justifyContent: 'center' }}>
        <img
          src={home_background}
          alt="Student with laptop"
          style={{ width: '50%', borderRadius: '8px' }}
        />
      </Box>
    </Box>
  );
};
