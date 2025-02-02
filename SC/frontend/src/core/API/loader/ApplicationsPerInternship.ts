import { IInternshipApi } from '../internship/IInternshipApi.ts';
import { ApplicationInfo } from '../../../models/application/application.ts';

export const ApplicationsPerInternship = async (
  api: IInternshipApi,
  internshipId: string,
  companyId: string
): Promise<ApplicationInfo[]> => {
  try {
    const applications = await api.getApplicationsPerInternship(
      internshipId,
      companyId
    );
    console.log(applications);
    return applications;
  } catch (error) {
    console.error(
      `Failed to load applications per internship ${internshipId}`,
      error
    );
    throw error;
  }
};
