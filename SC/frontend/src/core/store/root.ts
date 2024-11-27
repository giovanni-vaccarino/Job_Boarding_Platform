import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { authSlice } from './slices/auth';
import {
  FLUSH,
  PAUSE,
  PERSIST,
  persistReducer,
  persistStore,
  PURGE,
  REGISTER,
  REHYDRATE,
} from 'redux-persist';

import storage from 'redux-persist/lib/storage';
import { tabsSlice } from './slices/tabs.ts'; // defaults to localStorage for web

const persistConfig = {
  key: 'root-app-store',
  storage,
  whitelist: [authSlice.name],
};

const rootReducer = combineReducers({
  [authSlice.name]: authSlice.reducer,
  [tabsSlice.name]: tabsSlice.reducer,
});

const persistedReducer = persistReducer(persistConfig, rootReducer);

const appStore = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }),
});

const appActions = {
  [authSlice.name]: authSlice.actions,
  [tabsSlice.name]: tabsSlice.actions,
};

const persistor = persistStore(appStore);
export { appStore, persistor, appActions };
