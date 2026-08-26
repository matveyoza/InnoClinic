import { useState } from 'react';
import type { HeaderProps} from './Header.types';

export const Header = ({ user, onToggleSidebar }: HeaderProps) => {
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);

    return (
        <div className="flex-row justify-between px-6 py-3 bg-white border-b border-slate-200">
            <button
                onClick={onToggleSidebar}
                className="px-4 py-2 text-sm text-white font-medium cursor-pointer border border-slate-300 rounded-md bg-emerald-500 hover:bg-emerald-600 shadow-xl transition-colors"
            >
                ☰ Menu
            </button>

            <div className="relative flex justify-end">
                <img
                    src={user.avatarUrl}
                    alt={user.name}
                    onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                    className="-mt-8.5 flex justify-end w-20 h-20 rounded-full cursor-pointer border-2 border-emerald-50"
                />

                {isDropdownOpen && (
                    <div className="absolute right-0 top-12 bg-white shadow-lg rounded-lg p-3 w-40 border border-slate-100 z-20">
                        <div className="mb-1">
                            <strong className="block text-slate-800 text-sm">{user.name}</strong>
                            <small className="block text-slate-500 text-xs">{user.role}</small>
                        </div>

                        <hr className="my-2 border-slate-100" />

                        <button className="block w-full text-left px-2 py-1.5 text-sm text-slate-700 hover:bg-slate-50 rounded cursor-pointer">
                            Profile
                        </button>
                        <button className="block w-full text-left px-2 py-1.5 text-sm text-slate-700 hover:bg-slate-50 rounded cursor-pointer">
                            Settings
                        </button>
                        <button className="block w-full text-left px-2 py-1.5 text-sm text-red-500 hover:bg-slate-50 rounded cursor-pointer">
                            Log Out
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};
       