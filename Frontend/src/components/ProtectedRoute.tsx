import { Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../store/useAuth";
import { useEffect } from "react";
import { api } from "../api/axios";


export const ProtectedRoute = () => {
    const isAuthenticated = useAuth(state => state.isAuthenticated);
    const loading = useAuth(state => state.loading);
    const setAuth = useAuth(state => state.setAuth);
    const setLoading = useAuth(state => state.setLoading);
    const user = useAuth(state => state.user);
    const navigate = useNavigate();

    useEffect(() => {
        const checkAuthStatus = async () => {
            try {
                const response = await api.get('/auth/check');
                setAuth(response.data || user, true);
            } catch (error) {
                setAuth(null, false);
            } finally {
                setLoading(false);
            }
        };

        checkAuthStatus();
    }, [setAuth, setLoading, user]);

    useEffect(() => {
        if (!loading && !isAuthenticated) {
            navigate("/login", { replace: true });
        }
    }, [loading, isAuthenticated, navigate]);

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
        return null;
    }

    return <Outlet />;
};