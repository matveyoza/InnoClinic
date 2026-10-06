import { useRouteError } from "react-router-dom";

export const GlobalError = () => {
    const error = useRouteError() as Error; 

    return (
        <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50">
            <h1 className="text-3xl font-bold text-red-600 mb-2">Oops!</h1>
            <p className="text-slate-600 mb-4">Something went wrong.</p>
            
            <div className="bg-red-100 text-red-800 p-4 rounded-md">
                {error?.message || "An unknown error occurred."}
            </div>
        </div>
    );
};