import { create } from 'zustand';
import type { User, UserRole } from '../types/user';

interface UserState {
    user: User | null;
    setUser: (user: User | null) => void;
    hasRole: (roles: UserRole | UserRole[]) => boolean;
}

export const useUserStore = create<UserState>((set, get) => ({
    user: null,
    setUser: (user) => set({ user }),
    hasRole: (roles) => {
    const user = get().user;
    if (!user) return false;
    const allowed = Array.isArray(roles) ? roles : [roles];
    return allowed.includes(user.role);
  },
}));