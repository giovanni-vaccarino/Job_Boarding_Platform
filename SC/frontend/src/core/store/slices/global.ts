import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';

export enum Tab {
  Offers,
  Matches,
  Activity,
}

interface GlobalState {
  tabHomePage: Tab;
  confirmMessage: string;
  searchMessage: string;
}

const initialState: GlobalState = {
  tabHomePage: Tab.Offers,
  confirmMessage: '',
  searchMessage: '',
};

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
  },
});
