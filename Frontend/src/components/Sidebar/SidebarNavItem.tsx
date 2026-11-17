import type { ElementType, ReactNode } from 'react';
import { NavLink } from 'react-router-dom';

interface SidebarNavItemProps {
  to: string;
  icon: ElementType;
  children: ReactNode;
  onClick?: () => void;
}

export const SidebarNavItem = ({
  to,
  icon: Icon,
  children,
  onClick,
}: SidebarNavItemProps) => (
  <NavLink
    to={to}
    onClick={onClick}
    className={({ isActive }) =>
      `flex items-center gap-3 px-3 py-2 rounded-md font-medium text-sm transition-colors no-underline ${
        isActive
          ? 'bg-emerald-600 text-white shadow-sm'
          : 'text-slate-700 hover:bg-emerald-100/60 hover:text-emerald-700'
      }`
    }
  >
    <Icon className="w-5 h-5 shrink-0" />
    {children}
  </NavLink>
);