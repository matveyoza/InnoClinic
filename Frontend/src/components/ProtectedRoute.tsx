import { Outlet, Navigate } from "react-router-dom";
import { useAuth } from "../store/useAuth";
import { useEffect } from "react";
import { api } from "../api/axios";
import { useUserStore } from "../store/useUserStore";

export const ProtectedRoute = () => {
    const isAuthenticated = useAuth(state => state.isAuthenticated);
    const loading = useAuth(state => state.loading);
    const setIsAuthenticated = useAuth(state => state.setIsAuthenticated);    const setLoading = useAuth(state => state.setLoading);
    const setUser = useUserStore(state => state.setUser);

    useEffect(() => {
        const checkAuthStatus = async () => {
            try {
                const response = await api.get('/auth/check');
                setUser(response.data);
                setIsAuthenticated(true);
            } catch (error) {
                setIsAuthenticated(false);
                setUser(null);
            } finally {
                setLoading(false);
            }
        };

        checkAuthStatus();
    }, [setIsAuthenticated, setLoading, setUser]);

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50">
                <div className="text-slate-500 font-medium animate-pulse">
                    Authenticating...
                </div>
            </div>
        );
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return <Outlet />;
};