import { injectable } from 'inversify';
import { ApiBase } from '../setup-api/api-base/ApiBase.ts';
import { IAssetsApi } from './IAssetsApi.ts';

@injectable()
export class AssetsApi extends ApiBase implements IAssetsApi {
  async getCvStudent(studentId: string): Promise<any> {
    return await this.httpClient.get(`assets/${studentId}`);
  }
}
