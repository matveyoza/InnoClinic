import { useState } from "react";
import { login } from "../../services/authService";
import { Form, useActionData, redirect } from "react-router-dom";

export const loginAction = async ({ request }: any) => {
    const formData = await request.formData();
    const email = formData.get("email");
    const password = formData.get("password");

    try {
        await login({ email, password });

        return redirect("/main");
    } catch (err: any) {
        return { error: err.response?.data?.message || 'Invalid email or password.' };
    }
};

export const LogInPage = () => {
    const [showPassword, setShowPassword] = useState(false);

    const actionData = useActionData();

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-lg border border-slate-100 p-8">
                    <div className="text-center mb-8">
                        <h1 className="text-3xl font-bold text-slate-800">InnoClinic</h1>
                        <h2 className="text-slate-500 text-sm mt-2">Sign In</h2>
                    </div>

                {actionData?.error && (
                    <div className="mb-4 p-3 rounded-xl bg-red-50 border border-red-100 text-red-600 text-sm text-center">
                        {actionData.error}
                    </div>
                )}

                <Form method="post" className="space-y-5">
                    <div>
                        <label
                            htmlFor="email"
                            className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Email Adress
                        </label>
                        <input
                            id="email"
                            name="email"
                            type="email"
                            className="w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 text-slate-800 text-sm transition-all"
                            required
                        />
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
                                name="password"
                                type={showPassword ? 'text' : 'password'}
                                className="w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 text-slate-800 text-sm transition-all"
                                required
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-xs font-medium text-slate-400 hover:text-slate-600 transition-colors"
                            >
                                {showPassword ? 'Hide' : 'Show'}
                            </button>
                        </div>
                    </div>

                    <div className="flex items-center justify-between text-sm">
                        <label className="flex items-center text-slate-600 cursor-pointer">
                            <input
                                type="checkbox"
                                className="rounded border-slate-300 text-emerald-600 focus:ring-emerald-500 mr-2"
                            />
                            Remember me
                        </label>
                        <span className="text-emerald-600 font-medium hover:underline">
                            Forgot password?
                        </span>
                    </div>

                    <button
                    type="submit"
                    className="w-full py-3 px-4 bg-emerald-600 hover:bg-emerald-700 text-white font-medium rounded-xl shadow-md shadow-emerald-600/20 transition-all cursor-pointer"
                    >
                        Sign In
                    </button>
                </Form>

                <p className="flex justify-between text-sm text-slate-500 mt-6">
                    
                    Don't have an account?
                    <span className="text-right text-emerald-600 font-semibold hover:underline">
                        Register
                    </span>
                </p>
            </div>
        </div>
    );
};