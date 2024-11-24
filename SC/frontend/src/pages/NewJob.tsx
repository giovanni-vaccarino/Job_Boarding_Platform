import { useState } from 'react';
import { Checkbox, FormControlLabel, Box, Typography, FormGroup } from '@mui/material';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Page } from '../components/layout/Page.tsx'


export const NewJob = () => {




  return (
    <Page>
      <TitleHeader title={'Create a Job'} />
      <Box sx = {{
        display: 'flex',
        flexDirection: 'column',
        width: 'auto'
      }}>
        <Typography></Typography>
      </Box>
    </Page>


  );
}
