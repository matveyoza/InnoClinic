import { create } from 'zustand';
import { api } from '../api/axios'
import type { User } from '../types/user';

interface AuthState {
    user: User | null;
    loading: boolean;
    isAuthenticated: boolean;
    login: (credentials: { email: string; password: string }) => Promise<void>;
    logout: () => Promise<void>;
    checkAuthStatus: () => Promise<void>;
    setUser: (user: User | null) => void;
}

export const useAuthStore = create<AuthState>((set) =>({
    user: null,
    loading: true,
    isAuthenticated: false,

    setUser: (user) => set({ user, isAuthenticated: !!user }),

    checkAuthStatus: async () => {
        try {
            const response = await api.get<User>('/auth/me');
            set({ user: response.data, isAuthenticated: true });
        } catch {
            set({ user: null, isAuthenticated: false });
        } finally {
            set({ loading: false });
        }
    },

    login: async (credentials) => {
        await api.post('auth/login', credentials);
        console.log('Login successful, fetching user data...');

        const meResponse = await api.get<User>('/auth/me');
        console.log('get users');

        set({ user: meResponse.data, isAuthenticated: true });
    },

    logout: async () => {
        try {
            await api.post('/auth/logout');
        } finally {
            set({ user: null, isAuthenticated: false});
        }
    }
}));