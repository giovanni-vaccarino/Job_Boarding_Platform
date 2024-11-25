import { Box } from '@mui/material';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Page } from '../components/layout/Page.tsx';
import { InsertMultipleChoice } from '../components/newJobComponents/InsertMultipleChoice.tsx';
import { InsertTextField } from '../components/newJobComponents/InsertTextField.tsx';

export const NewJob = () => {
  return (
    <Page>
      <TitleHeader title={'Create a Job'} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          width: '90%',
          padding: 3,
          mt: '3rem',
          borderRadius: '8px',
          boxShadow: '0px 2px 8px rgba(0, 0, 0, 0.1)',
          margin: 'auto',
        }}
      >
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'row',
          }}
        >
          <Box
            sx={{
              width: '100%',
              display: 'flex',
              flexDirection: 'column',
              marginRight: '2%',
            }}
          >
            <InsertTextField
              titleTextField={'Job Title'}
              isRequired={true}
              label={'Job Title'}
            />
            <InsertTextField
              titleTextField={'Job Location'}
              isRequired={true}
              label={'Job Location'}
            />
            <InsertTextField
              titleTextField={'Application Deadline'}
              isRequired={true}
              label={'Application Deadline'}
            />
          </Box>
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              width: '100%',
            }}
          >
            <InsertMultipleChoice
              titleMultipleChoice={'Job Category'}
              isRequired={false}
              label={'Job Category'}
            />
            <InsertMultipleChoice
              titleMultipleChoice={'Job Type'}
              isRequired={false}
              label={'Job Type'}
            />
            <InsertMultipleChoice
              titleMultipleChoice={'Skills'}
              isRequired={true}
              label={'Skills'}
            />
          </Box>
        </Box>
        <Box sx={{}}>
          <InsertTextField
            titleTextField={'Job Description'}
            isRequired={true}
            label={'Job Description'}
          />
        </Box>
      </Box>
    </Page>
  );
};
