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
  verified: boolean;
}

const initialState: AuthState = {
  accessToken: null,
  refreshToken: null,
  loggedIn: false,
  profileType: null,
  profileId: null,
  verified: false,
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
      state.verified = action.payload.verified;
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
      state.profileType = 0;
      state.verified = null;
    },
    setProfileType: (state, action: PayloadAction<SetProfileType>) => {
      state.profileType = action.payload.type;
    },
    setProfileId: (state, action: PayloadAction<SetProfileId>) => {
      state.profileId = action.payload.id;
    },
    setVerified: (state) => {
      state.verified = true;
    },
  },
});
