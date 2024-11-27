import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';

export enum Tabs {
  Offers,
  Matches,
  Activity,
}

interface TabsState {
  tabHomePage: Tabs;
}

const initialState: TabsState = {
  tabHomePage: Tabs.Offers,
};

export interface TabHomePageInput {
  newTab: Tabs;
}

export const tabsSlice = createSlice({
  name: 'tabs',
  initialState,
  reducers: {
    setHomePageTab: (state, action: PayloadAction<TabHomePageInput>) => {
      state.tabHomePage = action.payload.newTab;
    },
  },
});
