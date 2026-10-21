import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../store/useAuthStore";
import { useUserStore } from "../../store/useUserStore";
import axios from "axios";
import type { User } from "../../types/user";
import { authApi } from "../../api/axios";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

const loginSchema = z.object({
    email: z
        .string()
        .min(1, "Email address is required")
        .email("Please enter a valid email address"),
    password: z
        .string()
        .min(1, "Password is required")
        .min(8, "Password must be at least 8 characters long"),
    rememberMe: z.boolean(),
});

type LoginInputs = z.infer<typeof loginSchema>;

export const LogInPage = () => {
    const [showPassword, setShowPassword] = useState<boolean>(false);
    const [error, setError] = useState<string>('');
    const setIsAuthenticated = useAuthStore(state => state.setIsAuthenticated);
    const setUser = useUserStore(state => state.setUser);
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<LoginInputs>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: "",
            password: "",
            rememberMe: false,
        },
    });

    const submitLogin = async (data: LoginInputs) => {
        setError('');

        try {
            const response = await authApi.post<User>('/login', {
                email: data.email,
                password: data.password
            });

            setUser(response.data);
            setIsAuthenticated(true);
            navigate("/main");
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.message || 'Invalid email or password.');
            } else if (err instanceof Error) {
                setError(err.message);
            } else {
                setError('An unknown error occurred during login.');
            }
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-lg border border-slate-100 p-8">
                    <div className="text-center mb-8">
                        <h1 className="text-3xl font-bold text-slate-800">InnoClinic</h1>
                        <h2 className="text-slate-500 text-sm mt-2">Sign In</h2>
                    </div>

                {error && (
                    <div className="mb-4 p-3 rounded-xl bg-red-50 border border-red-100 text-red-600 text-sm text-center">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit(submitLogin)} className="space-y-5">
                    <div>
                        <label
                            htmlFor="email"
                            className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Email Address
                        </label>
                        <input
                            id="email"
                            placeholder="Email Address"
                            type="email"
                            className={`w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm transition-all ${
                                errors.email
                                    ? 'border-red-500 focus:ring-red-500/20 focus:border-red-500'
                                    : 'border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                            }`}
                            {...register("email", {
                                required: "Email address is required",
                                pattern: {
                                    value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                                    message: "Please enter a valid email address",
                                },
                            })}
                        />
                        {errors.email && (
                            <p className="text-red-500 text-xs font-medium mt-1.5">
                                {errors.email.message}
                            </p>
                        )}
                    </div>

                    <div>
                        <label
                        htmlFor="password"
                        className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Password
                        </label>
                        <div className="relative">
                            <input
                                id="password"
                                placeholder="Password"
                                type={showPassword ? 'text' : 'password'}
                                className={`w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm transition-all ${
                                    errors.password
                                        ? 'border-red-500 focus:ring-red-500/20 focus:border-red-500'
                                        : 'border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                                }`}
                                {...register("password", {
                                    required: "Password is required",
                                    minLength: {
                                        value: 8,
                                        message: "Password must be at least 8 characters long",
                                    },
                                })}
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-xs font-medium text-slate-400 hover:text-slate-600 transition-colors"
                            >
                                {showPassword ? 'Hide' : 'Show'}
                            </button>
                        </div>
                        {errors.password && (
                            <p className="text-red-500 text-xs font-medium mt-1.5">
                                {errors.password.message}
                            </p>
                        )}
                    </div>

                    <div className="flex items-center justify-between text-sm">
                        <label className="flex items-center text-slate-600 cursor-pointer">
                            <input
                                type="checkbox"
                                className="rounded border-slate-300 text-emerald-600 focus:ring-emerald-500 mr-2"
                                {...register("rememberMe")}
                            />
                            Remember me
                        </label>
                        <Link to="/forgot-password"  className="text-emerald-600 font-medium hover:underline">
                            Forgot password?
                        </Link>
                    </div>

                    <button
                    type="submit"
                    disabled={isSubmitting}
                    className="w-full py-3 px-4 bg-emerald-600 hover:bg-emerald-700 text-white font-medium rounded-xl shadow-md shadow-emerald-600/20 transition-all cursor-pointer"
                    >
                        {isSubmitting ? "Signing In..." : "Sign In"}
                    </button>
                </form>

                <p className="flex justify-between text-sm text-slate-500 mt-6">
                    
                    Don't have an account?
                    <Link to="/register" className="text-right text-emerald-600 font-semibold hover:underline">
                        Register
                    </Link>
                </p>
            </div>
        </div>
    );
};