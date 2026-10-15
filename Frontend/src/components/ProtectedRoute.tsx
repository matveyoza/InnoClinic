import { Outlet, Navigate } from "react-router-dom";
import { useAuthStore } from "../store/useAuthStore";
import { useEffect } from "react";
import { userApi } from "../api/axios";
import { useUserStore } from "../store/useUserStore";

export const ProtectedRoute = () => {
    const isAuthenticated = useAuthStore(state => state.isAuthenticated);
    const loading = useAuthStore(state => state.loading);
    const setIsAuthenticated = useAuthStore(state => state.setIsAuthenticated);
    const setLoading = useAuthStore(state => state.setLoading);
    const setUser = useUserStore(state => state.setUser);

    useEffect(() => {
        const checkAuthStatus = async () => {
            try {
                const userResponse = await userApi.get('/current');
                setUser(userResponse.data);
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