import { FeedbackInternshipInput } from '../../../models/feedback/feedback.ts';

export interface IFeedbackApi {
  postFeedbackInternship: (input: FeedbackInternshipInput) => Promise<string>;
}
