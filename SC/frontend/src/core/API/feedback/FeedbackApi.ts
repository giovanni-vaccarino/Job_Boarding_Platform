import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import {
  FeedackPlatformInput,
  FeedbackInternshipInput,
} from '../../../models/feedback/feedback.ts';
import { IFeedbackApi } from './IFeedbackApi.ts';

@injectable()
export class FeedbackApi extends ApiBase implements IFeedbackApi {
  async postFeedbackInternship(
    input: FeedbackInternshipInput
  ): Promise<string> {
    return await this.httpClient.post<FeedbackInternshipInput, string>(
      'feedback/internship',
      input
    );
  }
  async postFeedbackPlatform(input: FeedackPlatformInput): Promise<string> {
    return await this.httpClient.post<FeedackPlatformInput, string>(
      'feedback/platform',
      input
    );
  }
}
