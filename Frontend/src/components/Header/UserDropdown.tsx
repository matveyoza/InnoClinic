import { useState } from 'react';
import type { User } from '../../types/user';
import guestAvatar from '../../assets/images/guest.jpg';
import { logout } from '../../services/authService';
import { SettingsModal } from '../Modals/SettingsModal';

interface UserDropdownProps {
    user: User | null;
}

export const UserDropdown = ({ user }: UserDropdownProps) => {
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [isSettingsOpen, setIsSettingsOpen] = useState<boolean>(false);

    const handleOpenSettings = () => {
        setIsOpen(false);
        setIsSettingsOpen(true);
    };

    return (
        <div>
            {isOpen && (
                <div
                    onClick={() => setIsOpen(false)}
                    className="fixed inset-0 bg-transparent z-40"
                />
            )}

            <div className="relative items-center flex justify-end h-28 px-6 z-50">
                <img
                    src={user?.avatarUrl || guestAvatar}
                    alt="User Avatar"
                    onClick={() => setIsOpen((prev) => !prev)}
                    className="w-20 h-20 rounded-full cursor-pointer border-2 border-emerald-50 object-cover"
                />

                {isOpen && (
                    <div className="absolute right-0 top-full bg-white shadow-lg rounded-lg p-3 w-40 border border-slate-100 z-20">
                        <div className="mb-1">
                            <strong className="block text-slate-800 text-sm">{user?.userName}</strong>
                            <small className="block text-slate-500 text-xs">{user?.email}</small>
                        </div>

                        <hr className="my-2 border-slate-100" />

                        <button className="block w-full text-left px-2 py-1.5 text-sm text-slate-700 hover:bg-slate-50 rounded cursor-pointer">
                            Profile
                        </button>
                        <button
                            className="block w-full text-left px-2 py-1.5 text-sm text-slate-700 hover:bg-slate-50 rounded cursor-pointer"
                            onClick={handleOpenSettings}
                        >
                            Settings
                        </button>
                        <button
                            className="block w-full text-left px-2 py-1.5 text-sm text-red-500 hover:bg-slate-50 rounded cursor-pointer"
                            onClick={logout}
                        >
                            Log Out
                        </button>
                    </div>
                )}
            </div>

            <SettingsModal 
                isOpen={isSettingsOpen} 
                onClose={() => setIsSettingsOpen(false)} 
            />
        </div>
    );
};