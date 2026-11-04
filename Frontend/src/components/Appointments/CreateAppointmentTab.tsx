import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import axios from "axios";
import { MdClose } from "react-icons/md";
import { AppointmentStatus } from "../../types/enums/appointmentStatus";
import type { Appointment } from "../../types/appointment";
import { appointmentApi } from "../../api/axios";
import { FormField } from "./FormField";
import { ClinicalDocumentsSection } from "./ClinicalDocumentsSection";

const appointmentDocumentSchema = z.object({
  id: z.string(),
  name: z.string(),
  dataType: z.string(),
  fileUrl: z.string(),
});

const createAppointmentSchema = z.object({
  doctorName: z.string().min(1, "Doctor name is required"),
  doctorSpecialty: z.string().min(1, "Specialty is required"),
  doctorAvatarUrl: z.string().url("Must be a valid URL").or(z.literal("")),
  date: z.string().min(1, "Date is required"),
  time: z.string().min(1, "Time is required"),
  room: z.number().min(1, "Room number must be greater than 0"),
  status: z.nativeEnum(AppointmentStatus),
  documents: z.array(appointmentDocumentSchema),
});

type CreateAppointmentInputs = z.infer<typeof createAppointmentSchema>;

interface CreateAppointmentTabProps {
  isOpen: boolean;
  onClose: () => void;
  onAppointmentCreated: (createdAppointment: Appointment) => void;
}

export const CreateAppointmentTab = ({isOpen, onClose, onAppointmentCreated }: CreateAppointmentTabProps) => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    reset,
    watch,
    setValue,
    formState: { errors },
  } = useForm<CreateAppointmentInputs>({
    resolver: zodResolver(createAppointmentSchema),
    defaultValues: {
      doctorName: "",
      doctorSpecialty: "",
      doctorAvatarUrl: "",
      date: "",
      time: "",
      room: 101,
      status: AppointmentStatus.Upcoming,
      documents: [],
    },
  });

  const documents = watch("documents");

  if (!isOpen)
    return null;

  const handleClose = () => {
    if (isSubmitting)
      return;
    setError(null);
    reset();
    onClose();
  };

  const submitAppointment = async (data: CreateAppointmentInputs) => {
    setIsSubmitting(true);
    setError(null);

    const payload = {
      doctorName: data.doctorName,
      doctorSpecialty: data.doctorSpecialty,
      doctorAvatarUrl: data.doctorAvatarUrl || null,
      date: data.date,
      time: data.time.length === 5 ? `${data.time}:00` : data.time,
      room: data.room,
      status: data.status,
      documents: data.documents.map((d) => ({
        name: d.name,
        type: d.dataType,
        url: d.fileUrl,
      })),
    };

    try {
      const response = await appointmentApi.post<Appointment>("/", payload);

      onAppointmentCreated(response.data);
      reset();
      onClose();
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(err.response?.data?.message || "Failed to create appointment.");
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("An unknown error occurred while saving.");
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div
        className="fixed inset-0 bg-slate-900/40 backdrop-blur-xs transition-opacity"
        onClick={handleClose}
      />

      <aside className="relative w-full max-w-lg max-h-[90vh] bg-white rounded-2xl shadow-2xl flex flex-col z-10 overflow-hidden">
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">New Appointment</h2>
            <p className="text-xs text-slate-400 mt-0.5">Fill in appointment and clinical details</p>
          </div>
          <button
            type="button"
            onClick={handleClose}
            disabled={isSubmitting}
            className="p-2 text-slate-400 hover:text-slate-600 rounded-lg hover:bg-slate-100 transition-colors cursor-pointer disabled:opacity-50"
          >
            <MdClose className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit(submitAppointment)} className="flex-1 overflow-y-auto p-6 space-y-4">
          {error && (
            <div className="p-3 text-xs text-rose-700 bg-rose-50 border border-rose-200 rounded-lg">
              {error}
            </div>
          )}

          <FormField
            label="Doctor Name"
            placeholder="e.g. Dr. Sarah Jenkins"
            disabled={isSubmitting}
            registration={register("doctorName")}
            error={errors.doctorName}
          />

          <div className="grid grid-cols-2 gap-4">
            <FormField
              label="Specialty"
              placeholder="e.g. Cardiologist"
              disabled={isSubmitting}
              registration={register("doctorSpecialty")}
              error={errors.doctorSpecialty}
            />

            <FormField
              label="Room Number"
              type="number"
              disabled={isSubmitting}
              registration={register("room", { valueAsNumber: true })}
              error={errors.room}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <FormField
              label="Date"
              type="date"
              disabled={isSubmitting}
              registration={register("date")}
              error={errors.date}
            />

            <FormField
              label="Time"
              type="time"
              disabled={isSubmitting}
              registration={register("time")}
              error={errors.time}
            />
          </div>

          <div>
            <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
              Initial Status
            </label>
            <select
              disabled={isSubmitting}
              className="w-full px-3.5 py-2 text-sm border border-slate-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600 bg-white disabled:bg-slate-50"
              {...register("status")}
            >
              <option value={AppointmentStatus.Upcoming}>Upcoming</option>
              <option value={AppointmentStatus.Completed}>Completed</option>
              <option value={AppointmentStatus.Cancelled}>Cancelled</option>
            </select>
          </div>

          <ClinicalDocumentsSection
            documents={documents}
            disabled={isSubmitting}
            onChange={(updatedDocs) => setValue("documents", updatedDocs, { shouldValidate: true })}
          />

          <div className="pt-4 border-t border-slate-100 flex items-center justify-end gap-3">
            <button
              type="button"
              onClick={handleClose}
              disabled={isSubmitting}
              className="px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-100 rounded-lg transition-colors cursor-pointer disabled:opacity-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="px-5 py-2 text-sm font-semibold text-white bg-emerald-600 hover:bg-emerald-700 rounded-lg shadow-sm transition-colors cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isSubmitting ? "Saving..." : "Save Appointment"}
            </button>
          </div>
        </form>
      </aside>
    </div>
  );
};