import { Box } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-title/TitleHeader.tsx';

export const Profile = () => {
  return (
    <Page>
      <Box>
        <TitleHeader title={'Profile'} />
      </Box>
    </Page>
  );
};
