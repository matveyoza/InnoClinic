import type { User } from '../../types/user';
import { MdMenu } from 'react-icons/md';
import { UserDropdown } from './UserDropdown';

interface HeaderProps {
    user: User | null;
    onToggleSidebar: () => void;
}

export const Header = ({ user, onToggleSidebar }: HeaderProps) => {
    return (
        <header className="flex justify-between items-center px-6 h-24 bg-white border-b border-slate-200 shrink-0">
            <button
                onClick={onToggleSidebar}
                className="flex items-center px-4 py-2 text-sm text-white font-medium cursor-pointer border border-slate-300 rounded-md bg-emerald-500 hover:bg-emerald-600 shadow-xl transition-colors"
            >
                <MdMenu className="w-5 h-5 mr-2" /> 
                Menu
            </button>

            <UserDropdown user={user} />
        </header>
    );
};
       