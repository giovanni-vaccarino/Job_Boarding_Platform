import { Question } from '../../../models/company/company.ts';
import { IInternshipApi } from '../internship/IInternshipApi.ts';

export const QuestionLoader = async (
  api: IInternshipApi,
  internshipId: string
): Promise<Question[]> => {
  try {
    const questions = await api.getInternshipQuestions(internshipId);
    console.log(questions);
    return questions;
  } catch (error) {
    console.error('Failed to load question, error');
    throw error;
  }
};
