import { IStudentApi } from '../student/IStudentApi.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';

export const ApplicationLoader = async (
  api: IStudentApi,
  studentId: string
): Promise<ApplicationInfo[]> => {
  try {
    const applications = await api.getApplications(studentId);
    console.log(applications);
    return applications;
  } catch (error) {
    console.error('Failed to load applications', error);
    throw error;
  }
};
