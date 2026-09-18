import { type FallbackProps } from "react-error-boundary";

export const GlobalErrorFallback = ({ error }: FallbackProps) => {
    return (
        <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50 p-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-lg border border-red-100 p-8 text-center">
                <h1 className="text-2xl font-bold text-red-600 mb-2">Fatal System Error</h1>
                <p className="text-slate-600 mb-4">
                    Something went completely wrong during initialization.
                </p>
                
                <pre className="text-xs text-left bg-slate-100 p-4 rounded overflow-auto text-red-500 mb-6">
                    {error instanceof Error ? error.message : String(error)}
                </pre>
                
                <button 
                    onClick={() => window.location.reload()} 
                    className="w-full py-3 px-4 bg-slate-800 hover:bg-slate-900 text-white font-medium rounded-xl shadow-md transition-all cursor-pointer"
                >
                    Reload Application
                </button>
            </div>
        </div>
    );
};