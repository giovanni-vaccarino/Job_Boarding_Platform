import 'reflect-metadata';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App.tsx';
import { IocContainerProvider } from './core/ioc/ioc-provider.tsx';
import { container } from './core/ioc/container.ts';
import { Provider } from 'react-redux';
import { appStore } from './core/store';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={appStore}>
      <IocContainerProvider container={container}>
        <App />
      </IocContainerProvider>
    </Provider>
  </StrictMode>
);
