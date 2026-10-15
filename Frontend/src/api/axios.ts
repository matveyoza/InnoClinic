import axios from 'axios';
import { AxiosError } from 'axios';
import { useAuthStore } from '../store/useAuthStore';
import { useUserStore } from '../store/useUserStore';

export const userApi = axios.create({
    baseURL: '/api/users',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const authApi = axios.create({
    baseURL: '/api/auth',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const setupAxiosInterceptors = () => {
  const handleUnauthorized = (error: AxiosError) => {
        const isLoginRequest = error.config?.url?.includes('/login');

        if (error.response?.status === 401 && !isLoginRequest) {
            useAuthStore.getState().setIsAuthenticated(false);
            useUserStore.getState().setUser(null);
        }
        return Promise.reject(error);
    };

    const authInterceptor = authApi.interceptors.response.use((res) => res, handleUnauthorized);
    const userInterceptor = userApi.interceptors.response.use((res) => res, handleUnauthorized);

    return () => {
        authApi.interceptors.response.eject(authInterceptor);
        userApi.interceptors.response.eject(userInterceptor);
    };
};