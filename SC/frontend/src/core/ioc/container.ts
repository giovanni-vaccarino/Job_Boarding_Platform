import { Container } from 'inversify';
import { ServiceType } from './service-type';
import { HttpClientFactory } from '../API/setup-api/factories/HttpClientFactory.ts';
import { IAuthApi } from '../API/auth/IAuthApi.ts';
import { AuthApi } from '../API/auth/AuthApi.ts';
import { ConfigServiceBuilder } from '../config/builder/ConfigServiceBuilder.ts';
import { EnvConfigProvider } from '../config/env-provider/EnvConfigProvider.ts';
import { IConfigService } from '../config/service/IConfigService.ts';
import { IConfigServiceBuilder } from '../config/builder/IConfigServiceBuilder.ts';
import { IInternshipApi } from '../API/internship/IInternshipApi.ts';
import { InternshipApi } from '../API/internship/InternshipApi.ts';
import { StudentApi } from '../API/student/StudentApi.ts';
import { IStudentApi } from '../API/student/IStudentApi.ts';
import { ICompanyApi } from '../API/company/ICompanyApi.ts';
import { CompanyApi } from '../API/company/CompanyApi.ts';
import { IMatchApi } from '../API/match/IMatchApi.ts';
import { MatchApi } from '../API/match/MatchApi.ts';
import { IFeedbackApi } from '../API/feedback/IFeedbackApi.ts';
import { FeedbackApi } from '../API/feedback/FeedbackApi.ts';
import { IAssetsApi } from '../API/assets/IAssetsApi.ts';
import { AssetsApi } from '../API/assets/AssetsApi.ts';

const container: Container = new Container();

// Config
const configBuilder: IConfigServiceBuilder = new ConfigServiceBuilder();
configBuilder.addProvider(new EnvConfigProvider());
container
  .bind<IConfigService>(ServiceType.Config)
  .toConstantValue(configBuilder.build());

// Api
container
  .bind<HttpClientFactory>(ServiceType.HttpClientFactory)
  .to(HttpClientFactory)
  .inSingletonScope();

container.bind<IAuthApi>(ServiceType.AuthApi).to(AuthApi).inSingletonScope();

container
  .bind<IInternshipApi>(ServiceType.InternshipApi)
  .to(InternshipApi)
  .inSingletonScope();

container
  .bind<IStudentApi>(ServiceType.StudentApi)
  .to(StudentApi)
  .inSingletonScope();

container
  .bind<ICompanyApi>(ServiceType.CompanyApi)
  .to(CompanyApi)
  .inSingletonScope();

container.bind<IMatchApi>(ServiceType.MatchApi).to(MatchApi).inSingletonScope();

container
  .bind<IFeedbackApi>(ServiceType.FeedbackApi)
  .to(FeedbackApi)
  .inSingletonScope();

container
  .bind<IAssetsApi>(ServiceType.AssetsApi)
  .to(AssetsApi)
  .inSingletonScope();

export { container };
