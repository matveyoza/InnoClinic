import type { User } from '../../types/user';

export interface HeaderProps {
    user: User;
    onToggleSidebar: () => void;
}