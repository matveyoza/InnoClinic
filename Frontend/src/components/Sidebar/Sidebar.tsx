import { Link } from "react-router-dom";
import { MdClose } from "react-icons/md";
import { MdCalendarMonth } from "react-icons/md";
import { MdAssignment } from 'react-icons/md';

interface SidebarProps {
    isOpen: boolean;
    onClose: () => void;
}

export const Sidebar =({ isOpen, onClose }: SidebarProps) => {
    return (
        <>
            <aside
                className={`fixed top-0 left-0 w-64 h-full bg-emerald-50 text-white z-30 p-6 transition-transform duration-300 ease-in-out ${
                    isOpen ? 'translate-x-0' : '-translate-x-full'
                }`}
            >
                <div className="flex justify-between items-center mb-8">
                    <h3 className="m-0 text-emerald-500 font-bold text-xl">InnoClinic</h3>
                    <button
                        onClick={onClose}
                        className="flex items-center justify-center bg-transparent border-none text-black text-lg cursor-pointer hover:text-slate-400 transition-colors"
                    >
                        <MdClose className="w-6 h-6" />
                    </button>
                </div>

                <nav className="flex flex-col gap-3">
                    <Link to="/schedule" className="text-black no-underline px-3 py-2 rounded-md hover:text-slate-400 transition-colors"
                    >
                        <MdCalendarMonth className="w-5 h-5 mr-2" />
                        Doctor Schedule
                    </Link>
                    <Link to="/appointments" className="text-black no-underline px-3 py-2 rounded-md hover:text-slate-400 transition-colors"
                    >
                        <MdAssignment className="w-5 h-5 mr-2" />
                        Appointments
                    </Link>
                </nav>
            </aside>

            {isOpen && (
                <div
                    onClick={onClose}
                    className="fixed inset-0 bg-black/40 z-25"
                />
            )}
        </>
    );
};