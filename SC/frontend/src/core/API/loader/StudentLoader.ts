import { IStudentApi } from '../student/IStudentApi.ts';
import { Student } from '../../../models/student/student.ts';

export const StudentLoader = async (
  api: IStudentApi,
  studentId: string
): Promise<Student> => {
  try {
    const studentInfo = await api.getStudentInfo(studentId);
    console.log(studentInfo);
    return studentInfo;
  } catch (error) {
    console.error('Failed to load studentInfo', error);
    throw error;
  }
};
