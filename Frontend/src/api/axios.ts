import axios from 'axios';
import { useAuth } from '../store/useAuth';
import { useUserStore } from '../store/useUserStore';

export const userApi = axios.create({
    baseURL: 'https://localhost:7183/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const authApi = axios.create({
    baseURL: 'https://localhost:7096/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const setupAxiosInterceptors = () => {
  const interceptorId = authApi.interceptors.response.use(
    (response) => response,
    (error) => {
      const isCheckEndpoint = error.config?.url?.includes('/auth/check');

      if (error.response?.status === 401 && !isCheckEndpoint) {
        useAuth.getState().setIsAuthenticated(false);
        useUserStore.getState().setUser(null);      }
      return Promise.reject(error);
    }
  );

  return () => authApi.interceptors.response.eject(interceptorId);
};