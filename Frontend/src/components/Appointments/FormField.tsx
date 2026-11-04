import type { InputHTMLAttributes } from "react";
import type { FieldError, UseFormRegisterReturn } from "react-hook-form";

interface FormFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string;
  registration: UseFormRegisterReturn;
  error?: FieldError;
}

export const FormField = ({
  label,
  registration,
  error,
  className = "",
  disabled,
  ...inputProps
}: FormFieldProps) => {
  return (
    <div>
      <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
        {label}
      </label>
      <input
        disabled={disabled}
        className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
          error
            ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
            : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
        } ${className}`}
        {...registration}
        {...inputProps}
      />
      {error && (
        <p className="text-red-500 text-xs font-medium mt-1">{error.message}</p>
      )}
    </div>
  );
};