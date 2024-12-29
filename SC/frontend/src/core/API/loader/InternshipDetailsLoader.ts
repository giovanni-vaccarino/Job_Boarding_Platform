import { IInternshipApi } from '../internship/IInternshipApi.ts';
import { Internship } from '../../../models/internship/internship.ts';

export const InternshipDetailsLoader = async (
  api: IInternshipApi,
  idInternship: string
): Promise<Internship> => {
  try {
    const internship = await api.getInternshipDetails(idInternship);
    console.log(internship);
    return internship;
  } catch (error) {
    console.error('Failed to load internships', error);
    throw error;
  }
};
