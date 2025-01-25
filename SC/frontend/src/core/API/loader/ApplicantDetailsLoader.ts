import { IInternshipApi } from '../internship/IInternshipApi.ts';
import { ApplicantInfo } from '../../../models/internship/internship.ts';

export const ApplicantDetailsLoader = async (
  api: IInternshipApi,
  applicationId?: string,
  studentId?: string,
  companyId?: string
): Promise<ApplicantInfo> => {
  try {
    const applicantInfo = await api.getApplicantInfo(
      applicationId,
      studentId,
      companyId
    );
    console.log(applicantInfo);
    return applicantInfo;
  } catch (error) {
    console.error('Failed to load applicant ${studentId} information', error);
    throw error;
  }
};
