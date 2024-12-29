import { useState } from 'react';
import { Box, Button, Typography } from '@mui/material';
import { Page } from '../components/layout/Page.tsx';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { RowComponent } from '../components/profile-components/RowComponent.tsx';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { ServiceType } from '../core/ioc/service-type.ts';
import { IStudentApi } from '../core/API/student/IStudentApi.ts';
import { ICompanyApi } from '../core/API/company/ICompanyApi.ts';
import { IAuthApi } from '../core/API/auth/IAuthApi.ts';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { Student } from '../models/student/student.ts';
import { useLoaderData } from 'react-router-dom';
import { Company } from '../models/company/company.ts';

export const Profile = () => {
  const studentApi = useService<IStudentApi>(ServiceType.StudentApi);
  const companyApi = useService<ICompanyApi>(ServiceType.CompanyApi);
  const authApi = useService<IAuthApi>(ServiceType.AuthApi);
  const navigate = useNavigateWrapper();

  const [selectedSection, setSelectedSection] = useState<string>('profile');
  //TODO to retrieve the account type
  const [accountType, setAccountType] = useState<string>('student');

  const [studentProfile, setStudentProfile] = useState({
    id: 10,
    email: 'student@mail.polimi.it',
    name: 'studentName',
    cf: '-',
    cvPath: '-',
    skills: ['skill1'],
    interests: ['interest1', 'interest2'],
  });

  const [companyProfile, setCompanyProfile] = useState({
    id: 10,
    email: 'company@mail.polimi.it',
    name: 'Amazon',
    vatNumber: '-',
    website: '-',
    linkedin: '-',
  });

  setStudentProfile(useLoaderData() as Student);
  setCompanyProfile(useLoaderData() as Company);

  const handleSectionChange = (section: string) => {
    setSelectedSection(section);
  };

  const handleTypeAccount = (type: string) => {
    setAccountType(type);
  };

  //The function is passed as a props to the row component and handle the update of the profile
  const handleFieldChange = async (
    fieldKey: string,
    value: string | string[]
  ) => {
    if (accountType === 'student') {
      setStudentProfile((prev) => ({
        ...prev,
        [fieldKey]: value, // Update specific field in student profile
      }));
      console.log('Updated Student Profile:', {
        ...studentProfile,
        [fieldKey]: value,
      });
      const res = await studentApi.updateStudentInfo(
        studentProfile.id.toString(),
        studentProfile
      );
      console.log(res);
    } else if (accountType === 'company') {
      setCompanyProfile((prev) => ({
        ...prev,
        [fieldKey]: value, // Update specific field in company profile
      }));
      console.log('Updated Company Profile:', {
        ...companyProfile,
        [fieldKey]: value,
      });
      const res = await companyApi.updateCompanyInfo(
        companyProfile.id.toString(),
        companyProfile
      );
      console.log(res);
    }
  };

  const renderContent = () => {
    if (selectedSection === 'profile') {
      if (accountType === 'student')
        return (
          <Box>
            <RowComponent
              label="eMail"
              value={studentProfile.email}
              fieldKey={'email'}
              buttons={['edit']}
              onFieldChange={handleFieldChange}
            />
            <RowComponent
              label="Name"
              value={studentProfile.name}
              buttons={['edit']}
              fieldKey={'name'}
              onFieldChange={handleFieldChange}
            />
          </Box>
        );
      else if (accountType === 'company')
        return (
          <Box>
            <RowComponent
              label="eMail"
              value={companyProfile.email}
              buttons={['edit']}
              fieldKey={'email'}
              onFieldChange={handleFieldChange}
            />
            <RowComponent
              label="Name"
              value={companyProfile.name}
              buttons={['edit']}
              fieldKey={'name'}
              onFieldChange={handleFieldChange}
            />
          </Box>
        );
    } else {
      if (selectedSection === 'info')
        if (accountType === 'student')
          return (
            <Box>
              <RowComponent
                label="CV"
                value={studentProfile.cvPath}
                buttons={['edit', 'view']}
                fieldKey={'cvPath'}
                onFieldChange={handleFieldChange}
              />
              <RowComponent
                label="Skills"
                value={studentProfile.skills}
                buttons={['edit']}
                fieldKey={'skills'}
                onFieldChange={handleFieldChange}
              />
              <RowComponent
                label="Interest"
                value={studentProfile.interests}
                buttons={['edit']}
                fieldKey={'interest'}
                onFieldChange={handleFieldChange}
              />
            </Box>
          );
        else if (accountType === 'company')
          return (
            <Box>
              <RowComponent
                label="Company Name"
                value={companyProfile.name}
                buttons={['edit']}
                fieldKey={'name'}
                onFieldChange={handleFieldChange}
              />
              <RowComponent
                label="Vat"
                value={companyProfile.vatNumber}
                buttons={['edit']}
                fieldKey={'vat'}
                onFieldChange={handleFieldChange}
              />
              <RowComponent
                label="Company Website"
                value={companyProfile.website}
                buttons={['edit']}
                fieldKey={'website'}
                onFieldChange={handleFieldChange}
              />
              <RowComponent
                label="Linkedin"
                value={companyProfile.linkedin}
                buttons={['edit']}
                fieldKey={'linkedin'}
                onFieldChange={handleFieldChange}
              />
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
          width: '70%', // Adjust the maximum width to accommodate two columns
          padding: 3,
          mt: '3rem',
          backgroundColor: '#FFFFFF',
          gap: '2rem',
        }}
      >
        {/* Left Column */}
        <Box
          sx={{
            position: 'fixed',
            minWidth: '30%',
            marginLeft: '0%',
            marginRight: '1%',
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

          <Box sx={{ borderTop: '2px solid gray', width: '20%', mb: '1rem' }} />

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

          <Box sx={{ borderTop: '2px solid gray', width: '20%', mt: '1rem' }} />

          {/* Logout */}
          <Button
            onClick={async () => {
              await authApi.logout();
              navigate(AppRoutes.Login);
            }}
          >
            <Typography
              sx={{
                color: 'red',
                fontSize: '1.35rem',
                fontWeight: '500',
                marginTop: '10%',
              }}
            >
              Logout
            </Typography>
          </Button>
        </Box>

        {/* Right Column */}
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            marginLeft: '40%', // Space between columns
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
