export interface Company {
  id: number;
  name: string;
  email: string;
  vatNumber: string;
  website: string;
  linkedin: string;
}

export interface Question {
  id: number;
  title: string;
  questionType: QuestionType;
  options: string[];
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
