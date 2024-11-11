import { Box, Typography } from '@mui/material';

export interface TitleHeaderProps {
  title: string;
}
export const TitleHeader = (props: TitleHeaderProps) => {
  return (
    <Box
      sx={{
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        py: '1.5rem',
        bgcolor: '#f4f5f7',
      }}
    >
      <Typography sx = {{color: "#000000", fontSize: "1.5rem", fontWeight: "500"}}>{props.title}</Typography>
    </Box>
  );
};
