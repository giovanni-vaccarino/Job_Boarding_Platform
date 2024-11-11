import { Container } from 'inversify';
import React, { PropsWithChildren, useContext, useMemo } from 'react';
import { ServiceType } from './service-type';

const iocContext = React.createContext<Container>(new Container());

export const useService = <TService,>(serviceSymbol: ServiceType) => {
  const container = useContext(iocContext);

  return useMemo(() => {
    if (Object.values(ServiceType).includes(serviceSymbol)) {
      return container.get<TService>(serviceSymbol);
    } else {
      throw new Error(`Service not found for: ${String(serviceSymbol)}`);
    }
  }, [serviceSymbol, container]);
};

interface IocContainerProviderProps {
  container: Container;
}

export const IocContainerProvider: React.FC<
  PropsWithChildren<IocContainerProviderProps>
> = (props) => {
  return (
    <iocContext.Provider value={props.container}>
      {props.children}
    </iocContext.Provider>
  );
};
