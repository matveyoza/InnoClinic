import { 
    FaTimes, 
    FaPalette, 
    FaGlobe, 
    FaBell, 
    FaQuestionCircle, 
    FaChevronRight 
} from 'react-icons/fa';

interface SettingsModalProps {
    isOpen: boolean;
    onClose: () => void;
}

export const SettingsModal = ({ isOpen, onClose }: SettingsModalProps) => {
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm z-50 flex items-center justify-center p-4">
            <div className="bg-white rounded-2xl max-w-md w-full p-6 shadow-2xl border border-slate-100 animate-in fade-in zoom-in-95 duration-150">
                <div className="flex justify-between items-center pb-4 border-b border-slate-100">
                    <div>
                        <h3 className="text-lg font-bold text-slate-800">Settings</h3>
                        <p className="text-xs text-slate-400">Manage your system preferences</p>
                    </div>
                    <button
                        onClick={onClose}
                        className="p-2 text-slate-400 hover:text-slate-600 hover:bg-slate-100 rounded-lg transition-colors cursor-pointer"
                        aria-label="Exit settings"
                    >
                        <FaTimes className="w-4 h-4" />
                    </button>
                </div>

                <div className="mt-4 space-y-2">
                    <button 
                        type="button"
                        className="w-full flex items-center justify-between p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-100 transition-all text-left cursor-pointer group"
                    >
                        <div className="flex items-center gap-3">
                            <div className="p-2.5 rounded-lg bg-emerald-50 text-emerald-600">
                                <FaPalette className="w-4 h-4" />
                            </div>
                            <div>
                                <p className="text-sm font-semibold text-slate-700">Theme</p>
                                <span className="text-xs text-slate-400">Light / Dark mode</span>
                            </div>
                        </div>
                        <FaChevronRight className="w-3.5 h-3.5 text-slate-300 group-hover:text-slate-500 transition-colors" />
                    </button>

                    <button 
                        type="button"
                        className="w-full flex items-center justify-between p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-100 transition-all text-left cursor-pointer group"
                    >
                        <div className="flex items-center gap-3">
                            <div className="p-2.5 rounded-lg bg-blue-50 text-blue-600">
                                <FaGlobe className="w-4 h-4" />
                            </div>
                            <div>
                                <p className="text-sm font-semibold text-slate-700">Language</p>
                                <span className="text-xs text-slate-400">English (US)</span>
                            </div>
                        </div>
                        <FaChevronRight className="w-3.5 h-3.5 text-slate-300 group-hover:text-slate-500 transition-colors" />
                    </button>

                    <button 
                        type="button"
                        className="w-full flex items-center justify-between p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-100 transition-all text-left cursor-pointer group"
                    >
                        <div className="flex items-center gap-3">
                            <div className="p-2.5 rounded-lg bg-amber-50 text-amber-600">
                                <FaBell className="w-4 h-4" />
                            </div>
                            <div>
                                <p className="text-sm font-semibold text-slate-700">Notifications</p>
                                <span className="text-xs text-slate-400">Alerts & appointment updates</span>
                            </div>
                        </div>
                        <FaChevronRight className="w-3.5 h-3.5 text-slate-300 group-hover:text-slate-500 transition-colors" />
                    </button>

                    <button 
                        type="button"
                        className="w-full flex items-center justify-between p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-100 transition-all text-left cursor-pointer group"
                    >
                        <div className="flex items-center gap-3">
                            <div className="p-2.5 rounded-lg bg-purple-50 text-purple-600">
                                <FaQuestionCircle className="w-4 h-4" />
                            </div>
                            <div>
                                <p className="text-sm font-semibold text-slate-700">Help & Support</p>
                                <span className="text-xs text-slate-400">FAQ and contact clinic support</span>
                            </div>
                        </div>
                        <FaChevronRight className="w-3.5 h-3.5 text-slate-300 group-hover:text-slate-500 transition-colors" />
                    </button>
                </div>

                <div className="mt-6 pt-4 border-t border-slate-100 flex justify-end">
                    <button
                        type="button"
                        onClick={onClose}
                        className="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 text-sm font-medium rounded-xl transition-colors cursor-pointer"
                    >
                        Exit
                    </button>
                </div>
            </div>
        </div>
    );
};