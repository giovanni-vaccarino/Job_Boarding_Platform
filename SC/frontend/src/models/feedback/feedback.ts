import { TypeProfile } from '../auth/register.ts';

export interface Feedback {
  id: string;
  text: string;
  rating: Rating;
}

export enum Rating{
    OneStar,
    TwoStars,
    ThreeStars,
    FourStars,
    FiveStars
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
