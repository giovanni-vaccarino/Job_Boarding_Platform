import { IStudentApi } from '../student/IStudentApi.ts';
import { Application } from '../../../models/application/application.ts';

export const ApplicationLoader = async (
    api: IStudentApi,
    studentId: string
): Promise<Application[]> => {
  try {
    studentId = "6";
    const applications = await api.getApplications(studentId);
    console.log(applications);
    return applications;
  } catch (error) {
    console.error('Failed to load applications', error);
    throw error;
  }
};
