import { IInternshipApi } from '../internship/IInternshipApi.ts';
import { Internship } from '../../../models/internship/internship.ts';

export const InternshipCompanyLoader = async (
  api: IInternshipApi,
  companyId: string
): Promise<Internship[]> => {
  try {
    const internships = await api.getInternshipCompany(companyId);
    console.log(internships);
    return internships;
  } catch (error) {
    console.error(`Failed to load internships of the ${companyId}`, error);
    throw error;
  }
};
