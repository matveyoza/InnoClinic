import { MdClose, MdCalendarMonth, MdAssignment } from "react-icons/md";
import { SidebarNavItem } from "./SidebarNavItem";

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

export const Sidebar = ({ isOpen, onClose }: SidebarProps) => {

  return (
    <>
      <aside
        className={`fixed top-0 left-0 w-64 h-full bg-emerald-50 z-30 p-6 transition-transform duration-300 ease-in-out ${
          isOpen ? "translate-x-0" : "-translate-x-full"
        }`}
      >
        <div className="flex justify-between items-center mb-8">
          <h3 className="m-0 text-emerald-600 font-bold text-xl">InnoClinic</h3>
          <button
            onClick={onClose}
            className="flex items-center justify-center bg-transparent border-none text-slate-700 text-lg cursor-pointer hover:text-slate-400 transition-colors"
          >
            <MdClose className="w-6 h-6" />
          </button>
        </div>

        <nav className="flex flex-col gap-2">
          <SidebarNavItem to="/main" icon={MdAssignment} onClick={onClose}>
            Appointments
          </SidebarNavItem>
          <SidebarNavItem to="/schedule" icon={MdCalendarMonth} onClick={onClose}>
            Doctor Schedule
          </SidebarNavItem>
        </nav>
      </aside>

      {isOpen && (
        <div
          onClick={onClose}
          className="fixed inset-0 bg-black/40 z-20"
        />
      )}
    </>
  );
};