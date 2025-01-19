import { IMatchApi } from '../match/IMatchApi.ts';
import { ApplicantDetailsProps } from '../../../models/student/student.ts';

export const ApplicantDetailsLoader = async (
  api: IMatchApi,
  studentId: string
): Promise<ApplicantDetailsProps> => {
  try {
    const applications = await api.getApplicantDetails(studentId);
    console.log(applications);
    return applications;
  } catch (error) {
    console.error('Failed to load applications', error);
    throw error;
  }
};
