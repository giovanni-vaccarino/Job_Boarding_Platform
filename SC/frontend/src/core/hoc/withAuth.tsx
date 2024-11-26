import { ComponentType } from 'react';
import { useAppSelector } from '../store';
import { Navigate } from 'react-router-dom';

export interface WithAuthProps {}

export function withAuth<T extends WithAuthProps = WithAuthProps>(
  WrappedComponent: ComponentType<T>
) {
  return (props: Omit<T, keyof WithAuthProps>) => {
    const loggedIn = useAppSelector((s) => s.auth.loggedIn);
    console.log({ loggedIn });
    if (loggedIn) {
      const newProps = { ...(props as T) };
      return <WrappedComponent {...newProps} />;
    } else {
      return <Navigate to={'/login'} />;
    }
  };
}
