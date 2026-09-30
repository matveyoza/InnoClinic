import axios from 'axios';
import { useAuth } from '../store/useAuth';

export const api = axios.create({
    baseURL: 'https://localhost:7096/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const setupAxiosInterceptors = () => {
  const interceptorId = api.interceptors.response.use(
    (response) => response,
    (error) => {
      const isCheckEndpoint = error.config?.url?.includes('/auth/check');

      if (error.response?.status === 401 && !isCheckEndpoint) {
        useAuth.getState().setAuth(null, false);
      }
      return Promise.reject(error);
    }
  );

  return () => api.interceptors.response.eject(interceptorId);
};