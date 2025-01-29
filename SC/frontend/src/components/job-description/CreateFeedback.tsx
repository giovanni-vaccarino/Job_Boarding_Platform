import { Box, Typography, TextField, Rating, Button } from '@mui/material';
import { appActions, useAppDispatch, useAppSelector } from '../../core/store';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';
import { FeedbackInternshipInput } from '../../models/feedback/feedback.ts';
import { useState } from 'react';
import { useService } from '../../core/ioc/ioc-provider.tsx';
import { IFeedbackApi } from '../../core/API/feedback/IFeedbackApi.ts';
import { ServiceType } from '../../core/ioc/service-type.ts';

export interface CreateFeedbackProps {
  applicationId: number;
}

export const CreateFeedback = (props: CreateFeedbackProps) => {
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();
  const authState = useAppSelector((state) => state.auth);
  const profileId = authState.profileId;
  const profileType = authState.profileType;
  const feedbackApi = useService<IFeedbackApi>(ServiceType.FeedbackApi);

  const [rating, setRating] = useState<number>(0);
  const [text, setText] = useState<string>('');

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        width: '100%',
        marginTop: '5%',
        gap: '1rem',
      }}
    >
      <Typography sx={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
        Feedback:
      </Typography>

      <Typography sx={{ fontSize: '1.1rem' }}>
        Describe the experience with the company:
      </Typography>

      <Box sx={{ marginTop: 2 }}>
        <TextField
          label="Your Description"
          variant="outlined"
          fullWidth
          multiline
          rows={4}
          placeholder="Type your answer here..."
          onChange={(e) => setText(e.target.value)}
        />
      </Box>

      <Typography sx={{ fontSize: '1.1rem' }}>Rate the company:</Typography>

      <Rating
        name="simple-controlled"
        size="large"
        value={rating} // Bind value to state
        onChange={(_event, newValue) => {
          setRating(newValue || 0); // Update state on change
        }}
      />
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
        }}
      >
        <Button
          variant="contained"
          color="primary"
          onClick={() => {
            const feedbackInternshipInput: FeedbackInternshipInput = {
              text: text,
              rating: rating,
              profileId: Number(profileId),
              applicationId: props.applicationId,
              actor: profileType,
            };

            const res = feedbackApi.postFeedbackInternship(
              feedbackInternshipInput
            );

            console.log(res);

            dispatch(
              appActions.global.setConfirmMessage({
                newMessage: 'Feedback Sent Successfully',
              })
            );
            navigate(AppRoutes.ConfirmPage);
          }}
          sx={{
            textTransform: 'none',
            borderRadius: 2,
            fontSize: '1.15rem',
            px: '1.7rem',
          }}
        >
          Send Feedback
        </Button>
      </Box>
    </Box>
  );
};
