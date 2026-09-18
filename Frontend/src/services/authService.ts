import { api } from '../api/axios';
import { useAuth } from '../store/useAuth';
import type { User } from '../types/user';

export const checkAuthStatus = async () => {
    const { setAuth, setLoading, user } = useAuth.getState();

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
    const { setAuth } = useAuth.getState();

    const response = await api.post<User>('/auth/login', credentials);
    setAuth(response.data, true);
};

export const logout = async () => {
    const { setAuth } = useAuth.getState();

    try {
        await api.post('/auth/logout');
    } finally {
        setAuth(null, false);
    }
};