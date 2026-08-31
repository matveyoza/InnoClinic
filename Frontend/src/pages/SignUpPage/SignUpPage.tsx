import { useState, type FormEvent } from "react";

export const SignUpPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirm, setPasswordConfirm] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [showPasswordConfirm, setShowPasswordConfirm] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (password !== passwordConfirm) {
            setErrorMessage("The passwords don't match");
            return;
        }

        setErrorMessage('');
        console.log('Signing up with:', { email, password });
    };

    const isPasswordMismatch = Boolean(errorMessage);

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-lg border border-slate-100 p-8">
                    <div className="text-center mb-8">
                        <h1 className="text-3xl font-bold text-slate-800">InnoClinic</h1>
                        <h2 className="text-slate-500 text-sm mt-2">Sign Up</h2>
                    </div>

                <form onSubmit={handleSubmit} className="space-y-5">
                    <div>
                        <label
                            htmlFor="email"
                            className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Email Adress
                        </label>
                        <input
                            id="email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 text-slate-800 text-sm transition-all"
                            required
                        />
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
                                type={showPassword ? 'text' : 'password'}
                                value={password}
                                onChange={(e) => {
                                    setPassword(e.target.value);
                                    if (errorMessage) setErrorMessage('');
                                }}
                                className={`w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 text-slate-800 text-sm transition-all ${
                                    isPasswordMismatch
                                        ? 'border-red-500 ring-2 ring-red-600/50 border-red-600'
                                        : 'border-slate-200 focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                                }`}  
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

                    <div>
                        <label
                        htmlFor="password"
                        className="block text-sm font-medium text-slate-700 mb-1"
                        >
                            Confirm password
                        </label>
                        <div className="relative">
                            <input
                                id="passwordConfirm"
                                type={showPasswordConfirm ? 'text' : 'password'}
                                value={passwordConfirm}
                                onChange={(e) => {
                                    setPasswordConfirm(e.target.value);
                                    if (errorMessage) setErrorMessage('');
                                }}
                                className={`w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500 text-slate-800 text-sm transition-all ${
                                    isPasswordMismatch
                                        ? 'border-red-500 ring-2 ring-red-600/50 border-red-600'
                                        : 'border-slate-200 focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-500'
                                }`}
                                required
                            />
                            <button
                                type="button"
                                onClick={() => setShowPasswordConfirm(!showPasswordConfirm)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-xs font-medium text-slate-400 hover:text-slate-600 transition-colors"
                            >
                                {showPasswordConfirm ? 'Hide' : 'Show'}
                            </button>
                        </div>

                        {errorMessage && (
                            <p className="text-red-500 text-xs font-medium mt-1.5">
                                {errorMessage}
                            </p>
                        )}
                    </div>

                    <div className="text-emerald-600 font-medium hover: text-sm">
                        <label className="flex items-center text-slate-600 cursor-pointer">
                            <input
                                type="checkbox"
                                className="rounded border-slate-300 text-emerald-600 focus:ring-emerald-500 mr-2"
                            />
                            Remember me
                        </label>
                    </div>

                    <button
                    type="submit"
                    className="w-full py-3 px-4 bg-emerald-600 hover:bg-emerald-700 text-white font-medium rounded-xl shadow-md shadow-emerald-600/20 transition-all cursor-pointer"
                    >
                        Sign Up
                    </button>
                </form>

                <p className="flex justify-between text-sm text-slate-500 mt-6">
                    
                    Have an account?
                    <a href="#sign-up" className="text-right text-emerald-600 font-semibold hover:underline">
                        Log In
                    </a>
                </p>
            </div>
        </div>
    );
};