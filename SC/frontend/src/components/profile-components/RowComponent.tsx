import { Box, Button, TextField, Typography } from '@mui/material';
import ModeIcon from '@mui/icons-material/Mode';
import RemoveRedEyeIcon from '@mui/icons-material/RemoveRedEye';
import { useState } from 'react';

export interface RowComponentProps {
  label: string;
  value: string;
  buttons: string[];
}

export const RowComponent: React.FC<RowComponentProps> = (
  props: RowComponentProps
) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editedValue, setEditedValue] = useState(props.value);

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        mb: '2rem',
        width: '20rem',
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row',
          justifyContent: 'space-between',
          flex: '1',
        }}
      >
        <Typography sx={{ fontSize: '1.2rem', fontWeight: '500' }}>
          {props.label}
        </Typography>
        {props.buttons.includes('edit') && (
          <Button
            variant="text"
            sx={{ marginRight: '20%' }}
            startIcon={<ModeIcon />}
            onClick={() => setIsEditing(true)}
          ></Button>
        )}
        {props.buttons.includes('view') && (
          <Button
            variant="text"
            sx={{ marginRight: '48%' }}
            startIcon={<RemoveRedEyeIcon />}
          ></Button>
        )}
      </Box>
      <Box sx={{ marginTop: '1%' }}>
        {isEditing ? (
          <>
            <TextField
              value={editedValue}
              onChange={(e) => setEditedValue(e.target.value)}
              fullWidth
            />
            <Button
              color="white"
              sx={{
                textTransform: 'none',
                borderRadius: 2,
                fontSize: '1.15rem',
                px: '1.7rem',
                backgroundColor: 'primary.main',
                mt: '1rem',
              }}
              onClick={() => {
                setIsEditing(false);
                // Update the actual value here if needed
              }}
            >
              Confirm change
            </Button>
          </>
        ) : (
          <Typography
            sx={{
              fontSize: '1.25rem',
              fontWeight: '500',
              color: 'rgba(0, 0, 0, 0.4)',
            }}
          >
            {editedValue}
          </Typography>
        )}
      </Box>
    </Box>
  );
};
