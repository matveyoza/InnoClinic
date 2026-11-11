import { create } from 'zustand';

interface AuthState {
    loading: boolean;
    isAuthenticated: boolean;
    setIsAuthenticated: (isAuthenticated: boolean) => void;
    setLoading: (loading: boolean) => void;
}

export const useAuthStore = create<AuthState>((set) =>({
    loading: true,
    isAuthenticated: false,

    setIsAuthenticated: (isAuthenticated) => set({ isAuthenticated }),
    setLoading: (loading) => set({ loading }),
}));