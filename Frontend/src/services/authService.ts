import { authApi } from '../api/axios';
import { useAuth } from '../store/useAuth';
import { useUser } from '../store/useUser';

export const logout = async () => {
    const setIsAuthenticated = useAuth.getState().setIsAuthenticated;
    const setUser = useUser.getState().setUser;
    try {
        await authApi.post('/auth/logout');
    } finally {
        setIsAuthenticated(false);
        setUser(null);
    }
};