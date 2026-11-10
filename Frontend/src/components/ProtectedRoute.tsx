import { Outlet, Navigate } from "react-router-dom";
import { useAuth } from "../store/useAuth";
import { useEffect } from "react";
import { userApi } from "../api/axios";
import { useUser } from "../store/useUser";
import type { UserRole } from "../types/user";

interface ProtectedRouteProps {
  allowedRoles?: UserRole[];
}

export const ProtectedRoute = ({ allowedRoles }: ProtectedRouteProps) => {
    const isAuthenticated = useAuth(state => state.isAuthenticated);
    const loading = useAuth(state => state.loading);
    const setIsAuthenticated = useAuth(state => state.setIsAuthenticated);
    const setLoading = useAuth(state => state.setLoading);
    const setUser = useUser(state => state.setUser);

    const user = useUser(state => state.user);

    useEffect(() => {
        if (isAuthenticated && user) {
            setLoading(false);
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

    if (allowedRoles && !user.roles.some((role) => allowedRoles.includes(role))) {
        return <Navigate to="/main" replace />;
    }

    return <Outlet />;
};