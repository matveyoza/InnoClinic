import { create } from 'zustand';
import type { User, UserRole } from '../types/user';

interface UserState {
  user: User | null;
  setUser: (user: User | null) => void;
  hasRole: (roles: UserRole | UserRole[]) => boolean;
}

export const useUserStore = create<UserState>((set, get) => ({
  user: null,
  setUser: (user) => {
    if (user) {
      const rawRoles = user.roles ?? ((user as unknown as { role?: UserRole }).role ? [(user as unknown as { role: UserRole }).role] : []);
      user.roles = Array.isArray(rawRoles) ? rawRoles : [rawRoles];
    }
    set({ user });
  },
  hasRole: (roles) => {
    const user = get().user;
    if (!user) return false;

    const userRoles: string[] = Array.isArray(user.roles)
      ? user.roles
      : (user as unknown as { role?: string }).role
        ? [(user as unknown as { role: string }).role]
        : [];

    if (userRoles.length === 0) return false;

    const allowed = (Array.isArray(roles) ? roles : [roles]).map((r) =>
      String(r).toLowerCase().trim()
    );

    return userRoles.some((role) =>
      allowed.includes(String(role).toLowerCase().trim())
    );
  },
}));