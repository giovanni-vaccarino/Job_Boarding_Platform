import { Box, Rating, Typography } from '@mui/material';
import { Rating as RatingValue } from "../../models/feedback/feedback.ts"

export interface RowComponentProps {
  feedbackText: string;
  rating: RatingValue;
}

export const ViewFeedback = (props: RowComponentProps) => {
    const rating : number = props.rating as number;
    console.log(rating);
    return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          gap: 1,
          marginTop: '10px',
        }}
      >
        <Typography sx={{ fontSize: '1.2rem', fontWeight: '500' }}>
          Describe the experience with the company
        </Typography>
        <Typography sx={{ fontSize: '1rem', fontWeight: '200' }}>
          {props.feedbackText}
        </Typography>
        <Typography
          sx={{ fontSize: '1.2rem', fontWeight: 'bold', marginTop: '20px' }}
        >
          Rate of the candidate
        </Typography>
        <Rating
          name="simple-controlled"
          value={Number(RatingValue[props.rating] + 1) }
          readOnly
          size="large"
        />
      </Box>
    </Box>
  );
};
