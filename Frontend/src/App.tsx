import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { LogInPage } from "./pages/LogInPage/LogInPage";
import { MainPage } from "./pages/MainPage/MainPage";
import { ProtectedRoute } from "./components/ProtectedRoute";

export const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LogInPage />} />
                <Route element={<ProtectedRoute />}>
                    <Route path="/main" element={<MainPage />} />
                </Route>
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </BrowserRouter>
    )
}