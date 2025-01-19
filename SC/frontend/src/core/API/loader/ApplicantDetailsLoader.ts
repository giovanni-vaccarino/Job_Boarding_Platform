import { IStudentApi } from '../student/IStudentApi.ts';
import { Application } from '../../../models/application/application.ts';

export const ApplicantDetailsLoader = async (
    api: IStudentApi,
    studentId: string
): Promise<ApplicantDetails> => {
    try {
        const applications = await api.getApplications(studentId);
        console.log(applications);
        return applications;
    } catch (error) {
        console.error('Failed to load applications', error);
        throw error;
    }
};
