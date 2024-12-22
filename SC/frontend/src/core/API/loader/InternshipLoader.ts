import { IInternshipApi } from '../internship/IInternshipApi.ts';
import { Internship } from '../../../models/internship/internship.ts';

export const InternshipLoader = async (
  api: IInternshipApi
): Promise<Internship[]> => {
  try {
    const internship = await api.getInternship();
    console.log(internship);
    return internship;
  } catch (error) {
    console.error('Failed to load internships', error);
    throw error;
  }
};
