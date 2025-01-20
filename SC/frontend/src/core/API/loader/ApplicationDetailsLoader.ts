import { IStudentApi } from '../student/IStudentApi.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';

export const ApplicationDetailsLoader = async (
  api: IStudentApi,
  studentId: string,
  applicationId: string
): Promise<ApplicationInfo> => {
  try {
    const applications = await api.getApplications(studentId);
    console.log(applications);

    for (const application of applications) {
      if (application.id.toString() == applicationId) {
        return application;
      }
    }
    return applications[0];
  } catch (error) {
    console.error('Failed to load applications', error);
    throw error;
  }
};
