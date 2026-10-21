import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import type { RegisterUser } from "../../types/registerUser";
import { authApi } from "../../api/axios";
import axios from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

const signUpSchema = z
    .object({
        username: z
            .string()
            .min(1, "Username is required")
            .min(3, "Username must be at least 3 characters"),
        email: z
            .string()
            .min(1, "Email address is required")
            .email("Please enter a valid email address"),
        password: z
            .string()
            .min(1, "Password is required")
            .min(8, "Password must be at least 8 characters"),
        passwordConfirm: z
            .string()
            .min(1, "Please confirm your password"),
        rememberMe: z.boolean(),
    })
    .refine((data) => data.password === data.passwordConfirm, {
        message: "The passwords don't match",
        path: ["passwordConfirm"],
    });

type SignUpInputs = z.infer<typeof signUpSchema>;

export const SignUpPage = () => {
    const [showPassword, setShowPassword] = useState<boolean>(false);
    const [showPasswordConfirm, setShowPasswordConfirm] = useState<boolean>(false);
    const [serverError, setServerError] = useState<string>('');
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<SignUpInputs>({
        resolver: zodResolver(signUpSchema),
        defaultValues: {
            username: "",
            email: "",
            password: "",
            passwordConfirm: "",
            rememberMe: false,
        },
    });

    const onSubmit = async (data: SignUpInputs) => {
        setServerError('');

        const payload = {
            username: data.username,
            email: data.email,
            password: data.password,
        };

        try {
            await authApi.post<RegisterUser>('/register', payload);
            navigate("/login");
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setServerError(err.response?.data?.message || 'Invalid email or password.');
            } else if (err instanceof Error) {
                setServerError(err.message);
            } else {
                setServerError('An unknown error occurred during registration.');
            }
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-lg border border-slate-100 p-8">
                <div className="text-center mb-8">
                    <h1 className="text-3xl font-bold text-slate-800">InnoClinic</h1>
                    <h2 className="text-slate-500 text-sm mt-2">Sign Up</h2>
                </div>

                {serverError && (
                    <div className="mb-4 p-3 rounded-xl bg-red-50 border border-red-100 text-red-600 text-sm text-center">
                        {serverError}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
                    <div>
                        <label
                            htmlFor="username"
                            className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Username
                        </label>
                        <input
                            id="username"
                            placeholder="Username"
                            type="text"
                            className={`w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm transition-all ${
                                errors.username
                                    ? 'border-red-500 focus:ring-red-500/20 focus:border-red-500'
                                    : 'border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                            }`}
                            {...register("username", {
                                required: "Username is required",
                            })}
                        />
                        {errors.username && (
                            <p className="text-red-500 text-xs font-medium mt-1.5">
                                {errors.username.message}
                            </p>
                        )}
                    </div>

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
                            Create password
                        </label>
                        <div className="relative">
                            <input
                                id="password"
                                placeholder="Create Password"
                                type={showPassword ? 'text' : 'password'}
                                className={`w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm transition-all ${
                                    errors.password
                                        ? 'border-red-500 focus:ring-red-500/20 focus:border-red-500'
                                        : 'border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                                }`}
                                {...register("password", {
                                    required: "Password is required",
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

                    <div>
                        <label
                            htmlFor="passwordConfirm"
                            className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Confirm password
                        </label>
                        <div className="relative">
                            <input
                                id="passwordConfirm"
                                placeholder="Confirm Password"
                                type={showPasswordConfirm ? 'text' : 'password'}
                                className={`w-full px-4 py-2.5 rounded-xl border text-slate-800 text-sm transition-all ${
                                    errors.passwordConfirm
                                        ? 'border-red-500 focus:ring-red-500/20 focus:border-red-500'
                                        : 'border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                                }`}
                                {...register("passwordConfirm")}
                            />
                            <button
                                type="button"
                                onClick={() => setShowPasswordConfirm(!showPasswordConfirm)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-xs font-medium text-slate-400 hover:text-slate-600 transition-colors"
                            >
                                {showPasswordConfirm ? 'Hide' : 'Show'}
                            </button>
                        </div>
                        {errors.passwordConfirm && (
                            <p className="text-red-500 text-xs font-medium mt-1.5">
                                {errors.passwordConfirm.message}
                            </p>
                        )}
                    </div>

                    <div className="text-emerald-600 font-medium text-sm">
                        <label className="flex items-center text-slate-600 cursor-pointer">
                            <input
                                type="checkbox"
                                className="rounded border-slate-300 text-emerald-600 focus:ring-emerald-500 mr-2"
                                {...register("rememberMe")}
                            />
                            Remember me
                        </label>
                    </div>

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full py-3 px-4 bg-emerald-600 hover:bg-emerald-700 disabled:bg-emerald-400 text-white font-medium rounded-xl shadow-md shadow-emerald-600/20 transition-all cursor-pointer"
                    >
                        {isSubmitting ? "Signing Up..." : "Sign Up"}
                    </button>
                </form>

                <p className="flex justify-between text-sm text-slate-500 mt-6">
                    Have an account?
                    <Link to="/login" className="text-right text-emerald-600 font-semibold hover:underline">
                        Log In
                    </Link>
                </p>
            </div>
        </div>
    );
};