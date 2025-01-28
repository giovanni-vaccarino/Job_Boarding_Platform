export interface Company {
  id: number;
  name: string;
  email: string;
  vatNumber: string;
  website: string;
  linkedin: string;
}

export interface UpdateCompany {
  name: string;
  vat: string;
  website: string;
}

export interface Question {
  id: number;
  title: string;
  questionType: QuestionType;
  options: string[];
  onChange: (value: string | string[] | boolean) => void;
}

export enum QuestionType {
  OpenQuestion,
  MultipleChoice,
  TrueOrFalse,
}

export interface QuestionsProps {
  questions: Question;
  id: number;
}

export interface AnswerResponse {
  questionId: number;
  answer: string[];
}

export interface AllAnswersResponse {
  questions: AnswerResponse[];
}
