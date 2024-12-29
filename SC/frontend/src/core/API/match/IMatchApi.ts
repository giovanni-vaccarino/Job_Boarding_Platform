import { Match } from '../../../models/match/match.ts';

export interface IMatchApi {
  getMatchesStudent: (studentId: string) => Promise<Match[]>;
  getMatchesCompany: (companyId: string) => Promise<Match[]>;
}
