import { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import { api, setupAxiosInterceptors } from "../api/axios";
import type { User } from "../types/user";

interface AuthContextType {
    isAuthenticated: boolean;
    user: User | null;
    loading: boolean;
    login: (credentials: { email: string; password: string }) => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);

    const checkAuthStatus = async () => {
        try {
            const response = await api.get<User>('/auth/me');
            setUser(response.data);
        } catch {
            setUser(null);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        const cleanupInterceptors = setupAxiosInterceptors(() => {
            setUser(null);
        });

        checkAuthStatus();

        return () => {
            cleanupInterceptors();
        };
    }, []);

    const login = async (credentials: { email: string; password: string }) => {
        await api.post('/auth/login', credentials);
        
        console.log('Login successful, fetching user data...');

        const meResponse = await api.get<User>('/auth/me');

        
        console.log('get users');
        setUser(meResponse.data);
    };

    const logout = async () => {
        try {
            await api.post('/auth/logout');
        } finally {
            setUser(null);
        }
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated: !!user, user, loading, login, logout}}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
        const context = useContext(AuthContext);
        if (!context) throw new Error('useAuth must be within an AuthProvider');
        return context;
};