import { authApi } from '../api/axios';
import { useAuthStore } from '../store/useAuthStore';
import { useUserStore } from '../store/useUserStore';

export const logout = async () => {
    const setIsAuthenticated = useAuthStore.getState().setIsAuthenticated;
    const setUser = useUserStore.getState().setUser;
    try {
        await authApi.post('/auth/logout');
    } finally {
        setIsAuthenticated(false);
        setUser(null);
    }
};