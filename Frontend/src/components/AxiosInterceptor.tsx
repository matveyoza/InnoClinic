import { useEffect } from 'react';
import { setupAxiosInterceptors } from '../api/axios';

export const AxiosInterceptor = ({ children }: { children: React.ReactNode }) => {
  useEffect(() => {
    const eject = setupAxiosInterceptors();
    return () => eject();
  }, []);

  return <>{children}</>;
};