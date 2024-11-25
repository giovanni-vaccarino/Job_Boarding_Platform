import { useState } from "react"
import { Box, Rating, Typography } from '@mui/material';


export interface RowComponentProps {
  feedbackText : string;
  rating : number;
}

export const ViewFeedback = (props : RowComponentProps) => {

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Box sx = {{
        display : "flex",
        flexDirection : "column",
        gap : 1,
        marginTop : "10px"
      }}>
        <Typography sx={{ fontSize: '1.3rem', fontWeight: '500' }}>
          Describe the experience with the company
        </Typography>
        <Typography sx={{ fontSize: '1rem', fontWeight: '200' }}>
          {props.feedbackText}
        </Typography>
        <Typography sx={{ fontSize: '1.3rem', fontWeight: 'bold', marginTop : '20px'}}>
          Rate of the candidate
        </Typography>
        <Rating
          name="simple-controlled"
          value={props.rating}
          readOnly
          size = 'large'
        />
      </Box>
    </Box>
  );
};
