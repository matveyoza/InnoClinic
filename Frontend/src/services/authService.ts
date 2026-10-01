import { api } from '../api/axios';
import { useAuth } from '../store/useAuth';
import { useUserStore } from '../store/useUserStore';
import type { User } from '../types/user';

export const login = async (credentials: { email: string; password: string }) => {
    const response = await api.post<User>('/auth/login', credentials);
    return response.data;
};

export const logout = async () => {
    const setIsAuthenticated = useAuth.getState().setIsAuthenticated;
    const setUser = useUserStore.getState().setUser;
    try {
        await api.post('/auth/logout');
    } finally {
        setIsAuthenticated(false);
        setUser(null);
    }
};