import { Typography } from '@mui/material';
import { JobDescriptionProps } from '../../models/application/application.ts';

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
        <strong>Job Category:</strong> {jobDescription.jobCategory} <br />
        <strong>Job Type:</strong> {jobDescription.jobType} <br />
        <strong>Location:</strong> {jobDescription.location} <br />
        <strong>
          Post Created:
        </strong> {jobDescription.postCreated.toString()} <br />
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
        <strong>Job description</strong>
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
    </>
  );
};
