import { authApi } from '../api/axios';
import { useAuthStore } from '../store/useAuth';
import { useUserStore } from '../store/useUser';

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