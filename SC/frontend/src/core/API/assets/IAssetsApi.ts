export interface IAssetsApi {
  getCvStudent: (studentId: string) => Promise<File>;
}
