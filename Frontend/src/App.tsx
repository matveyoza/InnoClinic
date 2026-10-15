import { createBrowserRouter, RouterProvider, Navigate } from "react-router-dom";
import { LogInPage } from "./pages/LogInPage/LogInPage";
import { MainPage } from "./pages/MainPage/MainPage";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { GlobalError } from "./components/GlobalError";

const router = createBrowserRouter([
    {
        errorElement: <GlobalError />, 
        children: [
            {
                path: "/login",
                element: <LogInPage />,
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
        ]
    }
]);
export const App = () => {
    return <RouterProvider router={router} />; 
};