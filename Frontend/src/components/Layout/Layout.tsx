import { useState } from 'react';
import { Header } from '../../components/Header/Header';
import { Sidebar } from '../../components/Sidebar/Sidebar';
import type { User } from '../../types/user';

interface LayoutProps {
    children: React.ReactNode;
}

export const Layout = ({ children }: LayoutProps) => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const currentUser: User = {
    id: '1',
    name: 'Ilya Peshkur',
    role: 'Eblan',
    avatarUrl: 'https://i.pinimg.com/736x/33/65/28/3365285c27eb14f02a4e3d881d117a13.jpg',
  };

  return (
    <div className="flex flex-col h-screen bg-emerald-50 overflow-hidden">
        <Header
            user={currentUser}
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