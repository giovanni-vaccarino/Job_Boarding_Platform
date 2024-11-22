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
        padding: '0 5%',
        px: '10rem',
        paddingTop: '1rem',
        backgroundColor: '#f5f7f6',
        height: '23rem',
      }}
    >
      <Stack
        direction="column"
        spacing={2}
        width="50%"
        sx={{
          marginLeft: '8%',
          marginBottom: '2%',
        }}
      >
        <Typography
          sx={{
            fontSize: '3rem',
            textAlign: 'left',
            maxWidth: '60%',
            fontWeight: 'bolder',
            lineHeight: 1.15,
            marginBottom: '5%',
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
            marginBottom: '5%',
            color: '#555',
            fontSize: '0.9rem',
          }}
        >
          Hand-picked opportunities to work from home, remotely, freelance,
          full-time, part-time, contract and internships.
        </Typography>

        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            maxWidth: '100%',
            width: '70%%',
            marginTop: '2%',
            marginBottom: '20%',
          }}
        >
          <TextField
            variant="outlined"
            placeholder="Search by job title..."
            fullWidth
            sx={{
              backgroundColor: '#ffffff',
              borderRadius: '4px',
              maxWidth: '30rem',
            }}
          />
          <Button
            variant="contained"
            sx={{
              marginLeft: '-1rem',
              backgroundColor: 'primary.main',
              color: '#fff',
              fontSize: '1.1rem',
              px: '2rem',
              fontWeight: 'bold',
              height: '3.6rem',
              width: '8.5rem',
              borderRadius: '4px',
            }}
          >
            Search
          </Button>
        </Box>
      </Stack>

      <Box sx={{ width: '55%', display: 'flex', justifyContent: 'center' }}>
        <img
          src={home_background}
          alt="Student with laptop"
          style={{ width: '60%', borderRadius: '8px' }}
        />
      </Box>
    </Box>
  );
};
