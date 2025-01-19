import { Page } from '../components/layout/Page.tsx';
import { HomePageHeader } from '../components/page-headers/HomePageHeader.tsx';
import { Box, MenuItem, Select, Stack, Typography } from '@mui/material';
import { JobListItem } from '../components/list-items/JobListItem.tsx';
import { useAppSelector } from '../core/store';
import { useState } from 'react';
import { useLoaderData } from 'react-router-dom';
import {Internship} from "../models/internship/internship.ts";

const jobList = [
  {
    companyName: 'Google',
    title: 'Software Engineer',
    location: 'Chicago',
    dataCreated: new Date('2024-11-01'),
  },
  {
    companyName: 'Amazon',
    title: 'Software Engineer',
    location: 'Chicago',
    dateCreated: new Date('2024-10-20'),
  },
  {
    companyName: 'Amazon',
    title: 'Software Engineer',
    location: 'Chicago',
    dataCreated: new Date('2024-10-15'),
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

  const internship = useLoaderData() as Internship[];

  const filteredJobs = internship.filter((job) => {
    // Filter by search message
    const matchesSearch =
      job.title.toLowerCase().includes(searchMessage.toLowerCase()) ||
      job.title.toLowerCase().includes(searchMessage.toLowerCase()) ||
      job.location.toLowerCase().includes(searchMessage.toLowerCase());

    const matchesDate =
      postedDate === PostedDate.Everytime ||
      (postedDate === PostedDate.Today &&
        job.dataCreated.toDateString() === today.toDateString()) ||
      (postedDate === PostedDate.CurrentWeek &&
        job.dataCreated >= startOfWeek) ||
      (postedDate === PostedDate.CurrentMonth &&
        job.dataCreated >= startOfMonth);

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
              companyName={job.title}
              jobTitle={job.title}
              location={job.location}
              datePosted={new Date('2024-11-01')}
              id={job.id.toString()}
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
