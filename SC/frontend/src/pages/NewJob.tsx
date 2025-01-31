import { useState, useEffect } from 'react';
import { Box, Button } from '@mui/material';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Page } from '../components/layout/Page.tsx';
import { InsertMultipleChoice } from '../components/new-job-components/InsertMultipleChoice.tsx';
import { InsertTextField } from '../components/new-job-components/InsertTextField.tsx';
import { AppRoutes } from '../router.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import {
    AddInternshipDto,
    AddJobDetailsDto,
    AddQuestionDto, DurationType,
    JobCategory,
    JobType,
    SkillsType,
} from '../models/internship/internship.ts';
import { appActions, useAppDispatch } from '../core/store';
import { InsertMultipleChoiceMultiSelect } from '../components/new-job-components/InsertMultipleChoiceMultiSelect.tsx';
import { RowComponent } from '../components/profile-components/RowComponent.tsx';

export const NewJob = () => {
  const navigate = useNavigateWrapper();

  const dispach = useAppDispatch();

  const [jobTitle, setJobTitle] = useState('');
  const [jobLocation, setJobLocation] = useState('');
  const [jobDescription, setJobDescription] = useState('');

  const [day, setDay] = useState('');
  const [month, setMonth] = useState('');
  const [year, setYear] = useState('');
  const [isDateValid, setIsDateValid] = useState(false);

  const [jobCategory, setJobCategory] = useState<JobCategory | undefined>();
  const [jobType, setJobType] = useState<JobType | undefined>();
  const [skills, setSkills] = useState<string[]>([]);

  useEffect(() => {
    const validateDate = () => {
      const dayNum = parseInt(day, 10);
      const monthNum = parseInt(month, 10);
      const yearNum = parseInt(year, 10);

      if (
        !isNaN(dayNum) &&
        !isNaN(monthNum) &&
        !isNaN(yearNum) &&
        dayNum >= 1 &&
        dayNum <= 31 &&
        monthNum >= 1 &&
        monthNum <= 12 &&
        yearNum >= 1900 &&
        yearNum <= 2100
      ) {
        setIsDateValid(true);
      } else {
        setIsDateValid(false);
      }
    };

    validateDate();
  }, [day, month, year]);

  const requiredTextFilled =
    jobTitle.trim() !== '' &&
    jobLocation.trim() !== '' &&
    jobDescription.trim() !== '' &&
    isDateValid &&
    skills.length > 0;

  return (
    <Page>
      <TitleHeader title={'Create a Job'} />
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          width: '90%',
          padding: 3,
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
              gap: '1rem',
            }}
          >
            <InsertTextField
              titleTextField={'Job Title'}
              isRequired={true}
              label={'Job Title'}
              value={jobTitle}
              onChange={setJobTitle}
            />
            <InsertTextField
              titleTextField={'Job Location'}
              isRequired={true}
              label={'Job Location'}
              value={jobLocation}
              onChange={setJobLocation}
            />
            <Box
              sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: '0.5rem',
              }}
            >
              <Box
                sx={{
                  display: 'flex',
                  gap: '1rem',
                  justifyContent: 'space-between',
                }}
              >
                <InsertTextField
                  titleTextField={'Deadline'}
                  isRequired={true}
                  label={'Day'}
                  value={day}
                  onChange={(value) => setDay(value)}
                />
                <InsertTextField
                  titleTextField={<Box sx={{ height: '1.5rem' }} />}
                  isRequired={true}
                  label={'Month'}
                  value={month}
                  onChange={(value) => setMonth(value)}
                />
                <InsertTextField
                  titleTextField={<Box sx={{ height: '1.5rem' }} />}
                  isRequired={true}
                  label={'Year'}
                  value={year}
                  onChange={(value) => setYear(value)}
                />
              </Box>
            </Box>
          </Box>
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              width: '100%',
              gap: '1rem',
            }}
          >
            <InsertMultipleChoice
              titleMultipleChoice={'Job Category'}
              isRequired={true}
              label={'Job Category'}
              options={Object.values(JobCategory).filter(
                (value) => typeof value === 'string'
              )}
              onChange={(value) => setJobCategory(value as JobCategory)}
            />
            <InsertMultipleChoice
              titleMultipleChoice={'Job Type'}
              isRequired={true}
              label={'Job Type'}
              options={Object.values(JobType).filter(
                (value) => typeof value === 'string'
              )}
              onChange={(value) => setJobType(value as JobType)}
            />
            <RowComponent
              label="Skills"
              value={skills}
              buttons={['edit']}
              fieldKey={'skills'}
              onFieldChange={(_, value) => setSkills(Array.isArray(value) ? value : [])}
            />
            {/*<InsertMultipleChoiceMultiSelect*/}
            {/*  titleMultipleChoice={'Skills'}*/}
            {/*  isRequired={true}*/}
            {/*  label={'Skills'}*/}
            {/*  selectedValues={skills as string[]}*/}
            {/*  options={Object.values(SkillsType).filter(*/}
            {/*    (value) => typeof value === 'string'*/}
            {/*  )}*/}
            {/*  onChange={(value) => setSkills(value as SkillsType[])}*/}
            {/*/>*/}
          </Box>
        </Box>
        <Box sx={{ mt: '1rem' }}>
          <InsertTextField
            titleTextField={'Job Description'}
            isRequired={true}
            label={'Job Description'}
            value={jobDescription}
            onChange={setJobDescription}
          />
        </Box>
      </Box>

      {requiredTextFilled && (
        <Button
          onClick={() => {
            const requirements: string[] = [...skills];
            const date =
              year +
              '-' +
              String(month).padStart(2, '0') +
              '-' +
              String(day).padStart(2, '0');

            const newInternship: AddInternshipDto = {
              JobDetails: {
                Title: jobTitle,
                Duration: DurationType['TwoToThreeMonths'],
                Description: jobDescription,
                ApplicationDeadline: date,
                Location: jobLocation,
                JobCategory: jobCategory,
                JobType: jobType,
                Requirements: requirements,
              } as AddJobDetailsDto,
              Questions: [] as AddQuestionDto[],
              ExistingQuestions: [] as number[],
            };


            console.log('New Internship Object:', newInternship);

            dispach(
              appActions.global.setNewInternship({
                newInternship: newInternship,
              })
            );

            navigate(AppRoutes.NewJobQuestion);
          }}
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            fontSize: '1rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            marginTop: 2,
            marginBottom: 2,
            width: '5%',
          }}
        >
          Next
        </Button>
      )}
    </Page>
  );
};
