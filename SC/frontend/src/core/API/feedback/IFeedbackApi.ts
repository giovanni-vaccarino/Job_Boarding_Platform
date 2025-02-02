import {
  FeedackPlatformInput,
  FeedbackInternshipInput,
} from '../../../models/feedback/feedback.ts';

export interface IFeedbackApi {
  postFeedbackInternship: (input: FeedbackInternshipInput) => Promise<string>;
  postFeedbackPlatform: (input: FeedackPlatformInput) => Promise<string>;
}
