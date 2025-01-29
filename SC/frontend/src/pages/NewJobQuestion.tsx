import { useState } from 'react';
import { Box, Button, Select, MenuItem } from '@mui/material';
import { TitleHeader } from '../components/page-headers/TitleHeader.tsx';
import { Page } from '../components/layout/Page.tsx';
import { InsertOpenQuestion } from '../components/new-job-components/question/InsertOpenQuestion.tsx';
import { InsertMultipleChoiceQuestion } from '../components/new-job-components/question/InsertMultipleChoiceQuestion.tsx';
import { InsertTrueFalseQuestion } from '../components/new-job-components/question/InsertTrueFalseQuestion.tsx';
import { useNavigateWrapper } from '../hooks/use-navigate-wrapper.ts';
import { AppRoutes } from '../router.tsx';
import { appActions, useAppDispatch, useAppSelector } from '../core/store';
import {
  AddInternshipDto,
  AddQuestionDto,
} from '../models/internship/internship.ts';
import { useService } from '../core/ioc/ioc-provider.tsx';
import { ServiceType } from '../core/ioc/service-type.ts';
import { ICompanyApi } from '../core/API/company/ICompanyApi.ts';

export const NewJobQuestion = () => {
  const companyApi = useService<ICompanyApi>(ServiceType.CompanyApi);
  const navigate = useNavigateWrapper();
  const dispatch = useAppDispatch();

  const [selectedQuestionType, setSelectedQuestionType] = useState('');
  const [questions, setQuestions] = useState<any[]>([]);

  const newInternship = useAppSelector((state) => state.global.newInternship);

  const authState = useAppSelector((state) => state.auth);
  const companyId = authState.profileId!.toString();

  const [newJobQuestionsDict, setNewJobQuestionsDict] = useState<{
    [id: number]: AddQuestionDto;
  }>({});

  const handleSaveQuestion = (id: number, question: AddQuestionDto) => {
    setNewJobQuestionsDict((prevDict) => ({
      ...prevDict,
      [id]: question,
    }));
  };

  const handleRemoveQuestion = (id: number) => {
    setNewJobQuestionsDict((prevDict) => {
      const updatedDict = { ...prevDict };
      delete updatedDict[id]; // Remove question by ID
      return updatedDict;
    });
  };

  const handleAddQuestion = () => {
    const id = Object.keys(newJobQuestionsDict).length; // Generate unique ID

    if (selectedQuestionType === 'True/False') {
      setQuestions([
        ...questions,
        <InsertTrueFalseQuestion
          key={id}
          id={id}
          onSave={(id, question) => handleSaveQuestion(id, question)}
          onRemove={(id) => handleRemoveQuestion(id)}
        />,
      ]);
    } else if (selectedQuestionType === 'Multiple Choice') {
      setQuestions([
        ...questions,
        <InsertMultipleChoiceQuestion
          key={id}
          id={id}
          onSave={(id, question) => handleSaveQuestion(id, question)}
          onRemove={(id) => handleRemoveQuestion(id)}
        />,
      ]);
    } else if (selectedQuestionType === 'Open Question') {
      setQuestions([
        ...questions,
        <InsertOpenQuestion
          key={id}
          id={id}
          onSave={(id, question) => handleSaveQuestion(id, question)}
          onRemove={(id) => handleRemoveQuestion(id)}
        />,
      ]);
    }
    setSelectedQuestionType(''); // Reset selection
  };

  return (
    <Page>
      <TitleHeader title={'Create a Job - Questions'} />

      {/* Display dynamically added questions */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          gap: '2rem',
          marginTop: '2rem',
          width: '80%',
        }}
      >
        {questions}
      </Box>

      {/* Add Question Component */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'left',
          alignItems: 'center',
          gap: '2rem',
          marginTop: '2rem',
          width: '80%',
        }}
      >
        <Select
          value={selectedQuestionType}
          onChange={(e) => setSelectedQuestionType(e.target.value)}
          displayEmpty
          sx={{
            width: '20%',
            backgroundColor: '#f9f9f9',
            borderRadius: '8px',
            '& .MuiOutlinedInput-root': {
              borderRadius: '8px',
            },
          }}
        >
          <MenuItem value="" disabled>
            Select Question Type *
          </MenuItem>
          <MenuItem value="True/False">True/False</MenuItem>
          <MenuItem value="Multiple Choice">Multiple Choice</MenuItem>
          <MenuItem value="Open Question">Open Question</MenuItem>
        </Select>
        <Button
          onClick={handleAddQuestion}
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            fontSize: '1rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            padding: '0.5rem 1.3rem',
            '&:hover': {
              backgroundColor: 'primary.dark',
            },
          }}
        >
          Add Question
        </Button>
      </Box>

      {/* Post Job Button */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'flex-end',
          marginTop: '2rem',
        }}
      >
        <Button
          onClick={async () => {
            const updatedQuestions = Object.values(newJobQuestionsDict);

            const dto: AddInternshipDto = {
              JobDetails: newInternship?.JobDetails, // Copy JobDetails explicitly
              Questions: updatedQuestions, // Use updatedQuestions from the dictionary
              ExistingQuestions: [...(newInternship?.ExistingQuestions || [])],
            };

            dispatch(
              appActions.global.setNewInternship({
                newInternship: dto,
              })
            );
            console.log('Final newInternship:', dto);

            const res = await companyApi.addInternship(companyId, dto);

            console.log(res);

            dispatch(
              appActions.global.setConfirmMessage({
                newMessage: 'New Job Created',
              })
            );
            navigate(AppRoutes.ConfirmPage);
          }}
          sx={{
            backgroundColor: 'primary.main',
            color: '#FFFFFF',
            fontSize: '1.2rem',
            fontWeight: 'bold',
            borderRadius: '8px',
            marginBottom: 2,
            padding: '0.5rem 2rem',
            '&:hover': {
              backgroundColor: 'primary.dark',
            },
          }}
        >
          Post Job
        </Button>
      </Box>
    </Page>
  );
};
