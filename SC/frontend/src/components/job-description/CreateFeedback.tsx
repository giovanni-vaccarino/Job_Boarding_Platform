import { Box, Typography, TextField, Rating, Button } from '@mui/material';
import { appActions, useAppDispatch } from '../../core/store';
import { AppRoutes } from '../../router.tsx';
import { useNavigateWrapper } from '../../hooks/use-navigate-wrapper.ts';

export interface FeedbackOptions {
  selectable: boolean;
}
export const CreateFeedback = (props: FeedbackOptions) => {
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

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
        />
      </Box>

      <Typography sx={{ fontSize: '1.1rem' }}>Rate the company:</Typography>

      <Rating name="simple-controlled" size="large" />
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
        }}
      >
        {props.selectable && (
          <Button
            variant="contained"
            color="primary"
            onClick={() => {
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
        )}
      </Box>
    </Box>
  );
};
