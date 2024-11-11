import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import { appActions, appStore, persistor } from './root';

export type RootState = ReturnType<typeof appStore.getState>;
export type AppDispatch = typeof appStore.dispatch;

// Use throughout your app instead of plain `useDispatch` and `useSelector`
export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export { appStore, appActions, persistor };
