import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import {
  AddInternshipDto,
  AddJobDetailsDto,
  AddQuestionDto,
} from '../../../models/internship/internship.ts';

export enum Tab {
  Offers,
  Matches,
  Activity,
}

interface GlobalState {
  tabHomePage: Tab;
  confirmMessage: string;
  searchMessage: string;
  newInternship: AddInternshipDto;
}

const initialState: GlobalState = {
  tabHomePage: Tab.Offers,
  confirmMessage: '',
  searchMessage: '',
  newInternship: new (class implements AddInternshipDto {
    // @ts-ignore
    ExistingQuestions: number[];
    // @ts-ignore
    JobDetails: AddJobDetailsDto;
    // @ts-ignore
    Questions: AddQuestionDto[];
  })(),
};

export interface SetInternshipDtoInput {
  newInternship: AddInternshipDto;
}

export interface TabHomePageInput {
  newTab: Tab;
}
export interface ConfirmMessageInput {
  newMessage: string;
}

export interface SearchMessageInput {
  newSearchMessage: string;
}

export const globalSlice = createSlice({
  name: 'global',
  initialState,
  reducers: {
    setHomePageTab: (state, action: PayloadAction<TabHomePageInput>) => {
      state.tabHomePage = action.payload.newTab;
    },
    setConfirmMessage: (state, action: PayloadAction<ConfirmMessageInput>) => {
      state.confirmMessage = action.payload.newMessage;
    },
    setSearchMessage: (state, action: PayloadAction<SearchMessageInput>) => {
      state.searchMessage = action.payload.newSearchMessage;
    },
    setNewInternship: (state, action: PayloadAction<SetInternshipDtoInput>) => {
      state.newInternship = action.payload.newInternship;
    },
  },
});
