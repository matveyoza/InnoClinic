import { Outlet, Navigate } from "react-router-dom";
import { useAuthStore } from "../store/useAuthStore";
import { useEffect } from "react";
import { userApi } from "../api/axios";
import { useUserStore } from "../store/useUserStore";
import type { UserRole } from "../types/user";

interface ProtectedRouteProps {
  allowedRoles?: UserRole[];
}

export const ProtectedRoute = ({ allowedRoles }: ProtectedRouteProps) => {
    const isAuthenticated = useAuthStore(state => state.isAuthenticated);
    const loading = useAuthStore(state => state.loading);
    const setIsAuthenticated = useAuthStore(state => state.setIsAuthenticated);
    const setLoading = useAuthStore(state => state.setLoading);
    const setUser = useUserStore(state => state.setUser);

    const user = useUserStore(state => state.user);

    useEffect(() => {
        if (isAuthenticated && user) {
            return;
        }

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
    }, [setIsAuthenticated, setLoading, setUser, isAuthenticated, user]);

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50">
                <div className="text-slate-500 font-medium animate-pulse">
                    Authenticating...
                </div>
            </div>
        );
    }

    if (!isAuthenticated || !user) {
        return <Navigate to="/login" replace />;
    }

    if (allowedRoles && !allowedRoles.includes(user.role)) {
        return <Navigate to="/main" replace />;
    }

    return <Outlet />;
};