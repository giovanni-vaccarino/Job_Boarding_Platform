import { Box, Typography } from '@mui/material';
import { JobDescriptionProps } from '../../models/application/application.ts';
import { ViewFeedback } from '../applicant-detail-page/ViewFeedback.tsx';

export const JobDescriptionCore = (props: JobDescriptionProps) => {
  const jobDescription = props.jobDescription;
  return (
    <>
      <Typography
        sx={{
          fontSize: '1rem',
          lineHeight: '1.9rem',
        }}
      >
        <strong>Internship Category:</strong> {jobDescription.jobCategory}{' '}
        <br />
        <strong>Internship Type:</strong> {jobDescription.jobType} <br />
        <strong>Location:</strong> {jobDescription.location} <br />
        <strong>Post Created:</strong>{' '}
        {jobDescription.postCreated.toString().split('T')[0]} <br />
        <strong>Application Deadline:</strong>{' '}
        {jobDescription.applicationDeadline.toString()}
      </Typography>

      {/* Job description section */}
      <Typography
        sx={{
          fontSize: '1rem',
          mt: '1rem',
        }}
      >
        <strong>Internship description</strong>
        <br />
        {jobDescription.jobDescriptionMessage}
      </Typography>

      {/* Skills required section */}
      <Typography
        sx={{
          fontSize: '1rem',
          mt: '1rem',
        }}
      >
        <strong>Skills required</strong>
        <ul>
          {jobDescription.skillsRequired.map((skill, index) => (
            <li key={index}>{skill}</li>
          ))}
        </ul>
      </Typography>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          mt: '2rem',
          gap: 3,
        }}
      >
        {props.jobDescription.feedbacks?.length > 0 && (
          <>
            <Typography sx={{ fontSize: '2.0rem', fontWeight: '500' }}>
              Feedback:
            </Typography>
            {props.jobDescription.feedbacks?.map((feedback, index) => (
              <Box
                key={index}
                sx={{ display: 'flex', flexDirection: 'column' }}
              >
                <Typography
                  sx={{ fontSize: '1.2rem', fontWeight: 'bold' }}
                >{`${index + 1})`}</Typography>
                <ViewFeedback
                  feedbackText={feedback.text}
                  rating={feedback.rating}
                />
              </Box>
            ))}
          </>
        )}
      </Box>
    </>
  );
};
