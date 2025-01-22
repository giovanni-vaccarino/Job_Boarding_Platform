import { Box, Button, TextField, Typography } from '@mui/material';
import ModeIcon from '@mui/icons-material/Mode';
import RemoveRedEyeIcon from '@mui/icons-material/RemoveRedEye';
import React, { useState } from 'react';
import { useService } from '../../core/ioc/ioc-provider.tsx';
import { IStudentApi } from '../../core/API/student/IStudentApi.ts';
import { ServiceType } from '../../core/ioc/service-type.ts';
import { cvToSend } from '../../models/student/student.ts';
import { useAppSelector } from '../../core/store';
import axios from 'axios';

export interface RowComponentProps {
  label: string;
  value: string | string[]; // Accept string or array of strings
  buttons: string[];
  fieldKey?: string;
  onFieldChange: (fieldKey?: string, value?: string | string[]) => void;
}

export const RowComponent: React.FC<RowComponentProps> = (
  props: RowComponentProps
) => {
  console.log(props);
  const [isEditing, setIsEditing] = useState(false);
  const [editedValue, setEditedValue] = useState(props.value);

  const handleSave = () => {
    setIsEditing(false);
    props.onFieldChange(props.fieldKey, editedValue);
  };

  const handleChange = (index: number, value: string) => {
    if (Array.isArray(editedValue)) {
      const updatedValues = [...editedValue];
      updatedValues[index] = value;
      setEditedValue(updatedValues);
    }
  };

  const addNewItem = () => {
    if (Array.isArray(editedValue)) {
      setEditedValue([...editedValue, '']);
    }
  };

  const removeItem = (index: number) => {
    if (Array.isArray(editedValue)) {
      const updatedValues = editedValue.filter((_, i) => i !== index);
      setEditedValue(updatedValues);
    }
  };

  const studentApi = useService<IStudentApi>(ServiceType.StudentApi);
  const authState = useAppSelector((state) => state.auth);
  const studentId = authState.profileId;

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
        {props.buttons.includes('edit') && props.label != 'CV' ? (
          <Button
            variant="text"
            sx={{ marginRight: '20%' }}
            startIcon={<ModeIcon />}
            onClick={() => {
              setIsEditing(!isEditing);
            }}
          ></Button>
        ) : (
          !props.buttons.includes('edit') &&
          props.label == 'CV' && (
            <Box sx={{ marginLeft: '10px', marginTop: '5px' }}>
              {' '}
              {/* Adjust margins as needed */}
              <input
                type="file"
                accept="application/pdf"
                onChange={async (e) => {
                  if (e.target.files && e.target.files[0]) {
                    const cvToSend: cvToSend = { file: e.target.files[0] };
                    console.log(cvToSend);

                    try {
                      const res = await studentApi.loadCvStudent(
                        studentId as string,
                        cvToSend
                      );
                      console.log('Upload Successful:', res);
                    } catch (error) {
                      if (axios.isAxiosError(error)) {
                        console.error(
                          'Axios Error:',
                          error.response?.data || error.message
                        );
                      } else {
                        console.error('Unexpected Error:', error);
                      }
                    }
                  }
                }}
              />
            </Box>
          )
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
            {Array.isArray(editedValue) ? (
              <>
                {editedValue.map((item, index) => (
                  <Box
                    key={index}
                    sx={{ display: 'flex', alignItems: 'center', mb: '0.5rem' }}
                  >
                    <TextField
                      value={item}
                      onChange={(e) => handleChange(index, e.target.value)}
                      fullWidth
                      sx={{ marginRight: '0.5rem' }}
                    />
                    <Button
                      color="error"
                      onClick={() => removeItem(index)}
                      sx={{ minWidth: '40px' }}
                    >
                      X
                    </Button>
                  </Box>
                ))}
                <Button
                  sx={{
                    textTransform: 'none',
                    borderRadius: 2,
                    fontSize: '1rem',
                    px: '1rem',
                    mt: '0.5rem',
                  }}
                  onClick={addNewItem}
                >
                  Add Item
                </Button>
              </>
            ) : (
              <TextField
                value={editedValue as string}
                onChange={(e) => setEditedValue(e.target.value)}
                fullWidth
              />
            )}
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
                handleSave();
              }}
            >
              Confirm change
            </Button>
          </>
        ) : (
          <>
            {Array.isArray(editedValue) ? (
              <Typography
                sx={{
                  fontSize: '1.25rem',
                  fontWeight: '500',
                  color: 'rgba(0, 0, 0, 0.4)',
                }}
              >
                {editedValue.join(', ')}
              </Typography>
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
          </>
        )}
      </Box>
    </Box>
  );
};
