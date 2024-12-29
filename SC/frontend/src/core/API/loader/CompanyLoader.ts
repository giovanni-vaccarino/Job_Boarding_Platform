import { ICompanyApi } from '../company/ICompanyApi.ts';
import { Company } from '../../../models/company/company.ts';

export const CompanyLoader = async (
  api: ICompanyApi,
  companyId: string
): Promise<Company> => {
  try {
    const companyInfo = await api.getCompanyInfo(companyId);
    console.log(companyInfo);
    return companyInfo;
  } catch (error) {
    console.error('Failed to load internships', error);
    throw error;
  }
};
