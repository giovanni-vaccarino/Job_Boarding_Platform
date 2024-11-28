import { useState } from 'react';
import { Box, Button, Typography } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { RowComponent } from '../components/profile-components/RowComponent.tsx';

export const Profile = () => {
  const [selectedSection, setSelectedSection] = useState<string>('profile');

  const handleSectionChange = (section: string) => {
    setSelectedSection(section);
  };

  const [accountType, setAccountType] = useState<string>('company');

  const handleTypeAccount = (type: string) => {
    setAccountType(type);
  };

  const renderContent = () => {
    if (selectedSection === 'profile') {
      if (accountType === 'student')
        return (
          <Box>
            <RowComponent
              label="eMail"
              value="vittorio.palladino@mail.polimi.it"
              buttons={['edit']}
            />
            <RowComponent label="Name" value="vittorio" buttons={['edit']} />
          </Box>
        );
      else if (accountType === 'company')
        return (
          <Box>
            <RowComponent
              label="eMail"
              value="vittorio.palladino@mail.polimi.it"
              buttons={['edit']}
            />
            <RowComponent label="Name" value="vittorio" buttons={['edit']} />
          </Box>
        );
    } else {
      if (selectedSection === 'info')
        if (accountType === 'student')
          return (
            <Box>
              <RowComponent label="CV" value="-" buttons={['edit', 'view']} />
              <RowComponent label="Skills" value="-" buttons={['edit']} />
              <RowComponent
                label="Interest"
                value="vittorio"
                buttons={['edit']}
              />
            </Box>
          );
        else if (accountType === 'company')
          return (
            <Box>
              <RowComponent
                label="Company Name"
                value="Amazon"
                buttons={['edit']}
              />
              <RowComponent label="Vat" value="-" buttons={['edit']} />
              <RowComponent
                label="Company Website"
                value="-"
                buttons={['edit']}
              />
              <RowComponent label="Linkedin" value="-" buttons={['edit']} />
            </Box>
          );
    }
  };

  return (
    <Page>
      <TitleHeader title={'Profile'} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row', // Align children in a row for two columns
          justifyContent: 'space-between', // Distribute space evenly
          width: '100%',
          maxWidth: '1000px', // Adjust the maximum width to accommodate two columns
          padding: 3,
          mt: '3rem',
          backgroundColor: '#FFFFFF',
        }}
      >
        {/* Left Column */}
        <Box
          sx={{
            marginLeft: '20%',
            display: 'flex',
            flexDirection: 'column', // Stack items vertically
            justifyContent: 'center', // Center items vertically
            alignItems: 'center', // Center items horizontally
          }}
        >
          {/* Circular Button with Camera Icon */}
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'center',
              alignItems: 'center',
              marginBottom: '10%', // Space between sections
            }}
          >
            <Button
              variant="contained"
              //startIcon={<PhotoCameraSharpIcon sx={{ marginLeft: '30%' }} />}
              sx={{
                width: '60px', // Circular button dimensions
                height: '60px',
                borderRadius: '50%', // Make it a circle
              }}
            ></Button>
          </Box>

          {/* Profile Information */}
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center', // Center text items horizontally
              width: '100%',
            }}
          >
            {/*<HorizontalRuleSharpIcon*/}
            {/*  sx={{*/}
            {/*    fontSize: '1rem',*/}
            {/*    transform: 'scaleX(7)',*/}
            {/*    marginBottom: '5%',*/}
            {/*  }}*/}
            {/*/>*/}
            <Button
              onClick={() => handleSectionChange('profile')}
              sx={{ mb: '8px', textTransform: 'none' }} // Avoid uppercase transformation
            >
              <Typography
                sx={{
                  fontSize: '1.25rem',
                  fontWeight: selectedSection === 'profile' ? 'bold' : 'normal', // Bold only if selected
                  color: 'black', // Always black
                }}
              >
                Profile
              </Typography>
            </Button>
            <Button
              onClick={() => handleSectionChange('info')}
              sx={{ mb: '8px', textTransform: 'none' }} // Avoid uppercase transformation
            >
              <Typography
                sx={{
                  fontSize: '1.25rem',
                  fontWeight: selectedSection === 'info' ? 'bold' : 'normal', // Bold only if selected
                  color: 'black', // Always black
                }}
              >
                Info
              </Typography>
            </Button>
            {/*<HorizontalRuleSharpIcon*/}
            {/*  sx={{*/}
            {/*    fontSize: '1rem',*/}
            {/*    transform: 'scaleX(7)',*/}
            {/*    marginTop: '10%',*/}
            {/*  }}*/}
            {/*/>*/}
          </Box>

          {/* Logout */}
          <Typography
            sx={{
              color: 'red',
              fontSize: '1.35rem',
              fontWeight: '500',
              marginTop: '10%',
            }}
          >
            Log Out
          </Typography>
        </Box>

        {/* Right Column */}
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            marginRight: '40%', // Space between columns
            maxWidth: '100%', // Ensure columns don't exceed 50% width
            marginTop: '5%',
            justifyContent: 'space-between', // Distribute space evenly
          }}
        >
          {renderContent()}
        </Box>
      </Box>
    </Page>
  );
};
