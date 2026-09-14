import axios from 'axios';

export const api = axios.create({
    baseURL: 'https://localhost:7096/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const setupAxiosInterceptors = (onUnauthorized: () => void) => {
    const interceptorId = api.interceptors.response.use(
        (response) => response,
        (error) => {
            if (error.response && error.response.status === 401) {
                onUnauthorized();
            }
            return Promise.reject(error);
        }
    );

    return () => api.interceptors.response.eject(interceptorId);
};