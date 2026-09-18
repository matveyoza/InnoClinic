import { createBrowserRouter, RouterProvider, Navigate } from "react-router-dom";
import { loginAction, LogInPage } from "./pages/LogInPage/LogInPage";
import { MainPage } from "./pages/MainPage/MainPage";
import { ProtectedRoute } from "./components/ProtectedRoute";

const router = createBrowserRouter([
    {
        path: "/login",
        element: <LogInPage />,
        action: loginAction,
    },
    {
        element: <ProtectedRoute />,
        children: [
            {
                path: "/main",
                element: <MainPage />,
            }
        ]
    },
    {
        path: "*",
        element: <Navigate to="/login" />,
    }
]);
export const App = () => {
    return <RouterProvider router={router} />; 
};