import { Match } from '../../../models/match/match.ts';
import { IMatchApi } from '../match/IMatchApi.ts';

export const MatchesLoaderStudent = async (
  api: IMatchApi,
  studentId: string
): Promise<Match[]> => {
  try {
    const matches = await api.getMatchesCompany(studentId);
    console.log(matches);
    return matches;
  } catch (error) {
    console.error("Failed to load company' matches, error");
    throw error;
  }
};
