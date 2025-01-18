import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import { LoginResponse } from '../../../models/auth/login';
import { TypeProfile } from '../../../models/auth/register.ts';

interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  loggedIn: boolean;
  profileType: TypeProfile | null;
  profileId: string | null;
}

const initialState: AuthState = {
  accessToken: null,
  refreshToken: null,
  loggedIn: false,
  profileType: null,
  profileId: null,
};

export interface SetProfileType {
  type: TypeProfile;
}

export interface SetProfileId {
  id: string;
}

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    successLogin: (state, action: PayloadAction<LoginResponse>) => {
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.loggedIn = true;
      state.profileId = action.payload.profileId.toString();
    },
    failLogin: (state) => {
      state.refreshToken = null;
      state.accessToken = null;
      state.loggedIn = false;
    },
    logout: (state) => {
      state.refreshToken = null;
      state.accessToken = null;
      state.loggedIn = false;
    },
    setProfileType: (state, action: PayloadAction<SetProfileType>) => {
      state.profileType = action.payload.type;
    },
    setProfileId: (state, action: PayloadAction<SetProfileId>) => {
      state.profileId = action.payload.id;
    },
  },
});
