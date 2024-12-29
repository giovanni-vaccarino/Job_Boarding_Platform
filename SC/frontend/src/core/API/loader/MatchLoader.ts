import { IStudentApi } from '../student/IStudentApi.ts';
import { Match } from '../../../models/match/match.ts';

export const MatchLoader = async (
  api: IStudentApi,
  studentId: string
): Promise<Match[]> => {
  try {
    const matches = await api.getStudentMatches(studentId);
    console.log(matches);
    return matches;
  } catch (error) {
    console.error('Failed to load applications', error);
    throw error;
  }
};
