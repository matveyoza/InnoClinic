import { useState } from 'react';
import { Header } from '../../components/Header/Header';
import { Sidebar } from '../../components/Sidebar/Sidebar';
import type { User } from '../../types/user';

interface LayoutProps {
    children: React.ReactNode;
    user: User;
}

export const Layout = ({ children, user }: LayoutProps) => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  return (
    <div className="flex flex-col h-screen bg-emerald-50 overflow-hidden">
        <Header
            user={user}
            onToggleSidebar={() => setIsSidebarOpen(!isSidebarOpen)}
        />
        
        <Sidebar
            isOpen={isSidebarOpen}
            onClose={() => setIsSidebarOpen(false)}
        />

        <main className="flex-1 overflow-y-auto p-6 md:p-8">
            <div className="max-w-7xl mx-auto">
                {children}
            </div>
        </main>
    </div>
  );
};