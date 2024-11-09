import { FC, PropsWithChildren } from 'react';
import { Layout } from './Layout';

export const Page: FC<PropsWithChildren> = (props) => {
  return <Layout>{props.children}</Layout>;
};
