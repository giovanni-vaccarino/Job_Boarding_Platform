import { TypeProfile } from '../auth/register.ts';

export interface Feedback {
  description: string;
  rating: number;
}

export interface FeedbackInternshipInput {
  text: string;
  rating: number;
  profileId?: number;
  applicationId: number;
  actor: TypeProfile | null;
}

export interface FeedackPlatformInput {
  text: string;
  rating: number;
  profileId: number;
  actor: TypeProfile;
}
