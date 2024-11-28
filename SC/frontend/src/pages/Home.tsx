import { Page } from '../components/layout/Page.tsx';
import { HomePageHeader } from '../components/page-headers/HomePageHeader.tsx';
import {
  Box,
  MenuItem,
  Select,
  Stack,
  Typography,
} from '@mui/material';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { useAppSelector } from '../core/store';
import * as React from 'react';
import { useState } from 'react';

const jobList = [
  {
    companyName: 'Google',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-11-01'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-10-20'),
  },
  {
    companyName: 'Amazon',
    jobTitle: 'Software Engineer',
    location: 'Chicago',
    datePosted: new Date('2024-10-15'),
  },
];

export enum PostedDate {
  Today,
  CurrentWeek,
  CurrentMonth,
  Everytime,
}

export const Home = () => {
  const [postedDate, setPostedDate] = useState<PostedDate>(
    PostedDate.Everytime
  );
  const searchMessage = useAppSelector((s) => s.global.searchMessage);

  const today = new Date();
  const startOfWeek = new Date(today);
  startOfWeek.setDate(today.getDate() - today.getDay());
  const startOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);

  const filteredJobs = jobList.filter((job) => {
    // Filter by search message
    const matchesSearch =
      job.companyName.toLowerCase().includes(searchMessage.toLowerCase()) ||
      job.jobTitle.toLowerCase().includes(searchMessage.toLowerCase()) ||
      job.location.toLowerCase().includes(searchMessage.toLowerCase());

    // Filter by postedDate
    const matchesDate =
      postedDate === PostedDate.Everytime ||
      (postedDate === PostedDate.Today &&
        job.datePosted.toDateString() === today.toDateString()) ||
      (postedDate === PostedDate.CurrentWeek &&
        job.datePosted >= startOfWeek) ||
      (postedDate === PostedDate.CurrentMonth &&
        job.datePosted >= startOfMonth);

    return matchesSearch && matchesDate;
  });

  return (
    <Page>
      <HomePageHeader />

      <Typography sx={{ fontWeight: 'bold', fontSize: '1.7rem', mt: '1.5rem' }}>
        All Popular Job Listed
      </Typography>

      <Stack
        direction="column"
        spacing={4}
        mt={2}
        sx={{
          width: '100%',
          mt: '1rem',
          alignItems: 'center',
          pb: '4rem',
        }}
      >
        <Box sx={{ display: 'flex', gap: '2rem' }}>
          <Select
            value={postedDate}
            onChange={(e) => setPostedDate(e.target.value as PostedDate)}
            fullWidth
            variant="outlined"
            displayEmpty
            required
            sx={{
              backgroundColor: 'rgba(236, 241, 236, 1)',
              borderRadius: '7px',
              color: 'rgba(0, 0, 0, 0.6)',
              '& .MuiOutlinedInput-notchedOutline': {
                border: 'none',
              },
              '&:hover': {
                backgroundColor: 'rgba(220, 230, 220, 1)',
              },
              fontSize: '1rem',
              fontWeight: 'bold',
            }}
          >
            <MenuItem value="" disabled>
              Select PostedDate
            </MenuItem>
            <MenuItem value={PostedDate.Today}>Posted Date: Today</MenuItem>
            <MenuItem value={PostedDate.CurrentWeek}>
              Posted Date: CurrentWeek
            </MenuItem>
            <MenuItem value={PostedDate.CurrentMonth}>
              Posted Date: CurrentMonth
            </MenuItem>
            <MenuItem value={PostedDate.Everytime}>
              Posted Date: Everytime
            </MenuItem>
          </Select>
        </Box>

        {filteredJobs.length > 0 ? (
          filteredJobs.map((job, index) => (
            <JobListItem
              key={index}
              companyName={job.companyName}
              jobTitle={job.jobTitle}
              location={job.location}
              datePosted={job.datePosted}
            />
          ))
        ) : (
          <Typography sx={{ fontStyle: 'italic' }}>
            No matching jobs found
          </Typography>
        )}
      </Stack>
    </Page>
  );
};
