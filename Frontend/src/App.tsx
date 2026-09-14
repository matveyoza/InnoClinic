import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { LogInPage } from "./pages/LogInPage/LogInPage";
import { MainPage } from "./pages/MainPage/MainPage";
import { ProtectedRoute } from "./components/ProtectedRoute";

export const App = () => {
    return (
        <BrowserRouter>
            <AuthProvider>
                <Routes>
                    <Route path="/login" element={<LogInPage />} />
                    <Route element={<ProtectedRoute />}>
                        <Route path="/main" element={<MainPage />} />
                    </Route>
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </AuthProvider>    
        </BrowserRouter>
    )
}