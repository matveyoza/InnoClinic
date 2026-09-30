import { api } from '../api/axios';
import { useAuth } from '../store/useAuth';
import type { User } from '../types/user';

export const checkAuthStatus = async () => {
    const setAuth = useAuth(state => state.setAuth);
    const setLoading = useAuth(state => state.setLoading);
    const user = useAuth(state => state.user);

    try {
        await api.get<User>('/auth/check');
        setAuth(user, true);
    } catch (error) {
        setAuth(null, false);
    } finally {
        setLoading(false);
    }
};

export const login = async (credentials: { email: string; password: string }) => {
    const response = await api.post<User>('/auth/login', credentials);
    return response.data;
};

export const logout = async () => {
    const setAuth = useAuth(state => state.setAuth);

    try {
        await api.post('/auth/logout');
    } finally {
        setAuth(null, false);
    }
};