import { Typography } from '@mui/material';

export interface JobDescriptionProps {
  jobCategory: string;
  jobType: string;
  location: string;
  postCreated: Date;
  applicationDeadline: Date;
  jobDescription: string;
  skillsRequired: string[];
}

export const JobDescriptionCore = (props: JobDescriptionProps) => {
  return (
    <>
      <Typography
        sx={{
          fontSize: '1rem',
          lineHeight: '1.9rem',
        }}
      >
        <strong>Job Category:</strong> {props.jobCategory} <br />
        <strong>Job Type:</strong> {props.jobType} <br />
        <strong>Location:</strong> {props.location} <br />
        <strong>
          Post Created:
        </strong> {props.postCreated.toLocaleDateString()} <br />
        <strong>Application Deadline:</strong>{' '}
        {props.applicationDeadline.toLocaleDateString()}
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
        {props.jobDescription}
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
          {props.skillsRequired.map((skill, index) => (
            <li key={index}>{skill}</li>
          ))}
        </ul>
      </Typography>
    </>
  );
};
