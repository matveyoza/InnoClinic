import { create } from 'zustand';
import type { User } from '../types/user';

interface AuthState {
    user: User | null;
    loading: boolean;
    isAuthenticated: boolean;
    setAuth: (user: User | null, isAuthenticated: boolean) => void;
    setLoading: (loading: boolean) => void;
}

export const useAuth = create<AuthState>((set) =>({
    user: null,
    loading: true,
    isAuthenticated: false,

    setAuth: (user, isAuthenticated) => set({ user, isAuthenticated }),
    setLoading: (loading) => set({ loading }),
}));