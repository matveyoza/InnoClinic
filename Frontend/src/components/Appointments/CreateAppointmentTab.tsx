import { useRef } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { MdClose, MdAttachFile, MdDeleteOutline, MdDescription } from "react-icons/md";
import { AppointmentStatus } from "../../types/enums/appointmentStatus";
import type { Appointment } from "../../types/appointment";

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
  status: z.enum([
  AppointmentStatus.Upcoming,
  AppointmentStatus.Completed,
  AppointmentStatus.Cancelled,
]),
  documents: z.array(appointmentDocumentSchema),
});

type CreateAppointmentInputs = z.infer<typeof createAppointmentSchema>;

interface CreateAppointmentTabProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (appointment: Omit<Appointment, "id">) => void;
}

export const CreateAppointmentTab = ({ isOpen, onClose, onSave }: CreateAppointmentTabProps) => {
  const fileInputRef = useRef<HTMLInputElement | null>(null);

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

  if (!isOpen) return null;

  const handleFileUpload = (files: FileList | null) => {
    if (!files || files.length === 0) return;

    const newDocs = Array.from(files).map((file) => ({
      id: crypto.randomUUID(),
      name: file.name,
      dataType: file.type || "application/octet-stream",
      fileUrl: URL.createObjectURL(file),
    }));

    setValue("documents", [...documents, ...newDocs]);
    if (fileInputRef.current) fileInputRef.current.value = "";
  };

  const handleRemoveDoc = (id: string) => {
    setValue(
      "documents",
      documents.filter((doc) => doc.id !== id)
    );
  };

  const submitAppointment = (data: CreateAppointmentInputs) => {
    onSave({
      ...data,
      documents: data.documents.length > 0 ? data.documents : undefined,
    });
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div
        className="fixed inset-0 bg-slate-900/40 backdrop-blur-xs transition-opacity"
        onClick={onClose}
      />

      <aside className="relative w-full max-w-lg max-h-[90vh] bg-white rounded-2xl shadow-2xl flex flex-col z-10 overflow-hidden">
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">New Appointment</h2>
            <p className="text-xs text-slate-400 mt-0.5">Fill in appointment and clinical details</p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="p-2 text-slate-400 hover:text-slate-600 rounded-lg hover:bg-slate-100 transition-colors cursor-pointer"
          >
            <MdClose className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit(submitAppointment)} className="flex-1 overflow-y-auto p-6 space-y-4">
          <div>
            <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
              Doctor Name
            </label>
            <input
              placeholder="e.g. Dr. Sarah Jenkins"
              className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
                errors.doctorName
                  ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
                  : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
              }`}
              {...register("doctorName")}
            />
            {errors.doctorName && (
              <p className="text-red-500 text-xs font-medium mt-1">{errors.doctorName.message}</p>
            )}
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Specialty
              </label>
              <input
                placeholder="e.g. Cardiologist"
                className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
                  errors.doctorSpecialty
                    ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
                    : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
                }`}
                {...register("doctorSpecialty")}
              />
              {errors.doctorSpecialty && (
                <p className="text-red-500 text-xs font-medium mt-1">{errors.doctorSpecialty.message}</p>
              )}
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Room Number
              </label>
              <input
                type="number"
                className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
                  errors.room
                    ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
                    : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
                }`}
                {...register("room", { valueAsNumber: true })}
              />
              {errors.room && (
                <p className="text-red-500 text-xs font-medium mt-1">{errors.room.message}</p>
              )}
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Date
              </label>
              <input
                type="date"
                className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
                  errors.date
                    ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
                    : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
                }`}
                {...register("date")}
              />
              {errors.date && (
                <p className="text-red-500 text-xs font-medium mt-1">{errors.date.message}</p>
              )}
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Time
              </label>
              <input
                type="time"
                className={`w-full px-3.5 py-2 text-sm border rounded-lg transition-all ${
                  errors.time
                    ? "border-red-500 focus:ring-red-500/20 focus:border-red-500"
                    : "border-slate-200 focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600"
                }`}
                {...register("time")}
              />
              {errors.time && (
                <p className="text-red-500 text-xs font-medium mt-1">{errors.time.message}</p>
              )}
            </div>
          </div>

          <div>
            <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
              Initial Status
            </label>
            <select
              className="w-full px-3.5 py-2 text-sm border border-slate-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-600 bg-white"
              {...register("status")}
            >
              <option value={AppointmentStatus.Upcoming}>Upcoming</option>
              <option value={AppointmentStatus.Completed}>Completed</option>
              <option value={AppointmentStatus.Cancelled}>Cancelled</option>
            </select>
          </div>

          <div className="pt-2 border-t border-slate-100">
            <div className="flex items-center justify-between mb-2">
              <label className="text-xs font-semibold text-slate-600 uppercase">
                Clinical Documents
              </label>
              <button
                type="button"
                onClick={() => fileInputRef.current?.click()}
                className="inline-flex items-center gap-1.5 text-xs font-semibold text-emerald-600 hover:text-emerald-700 bg-emerald-50 hover:bg-emerald-100/70 px-2.5 py-1.5 rounded-md transition-colors cursor-pointer"
              >
                <MdAttachFile className="w-4 h-4" />
                Attach Doc
              </button>
              <input
                ref={fileInputRef}
                type="file"
                multiple
                className="hidden"
                accept=".pdf,.doc,.docx,.png,.jpg,.jpeg,.xml"
                onChange={(e) => handleFileUpload(e.target.files)}
              />
            </div>

            {documents.length > 0 ? (
              <ul className="space-y-2 mt-2">
                {documents.map((doc) => (
                  <li
                    key={doc.id}
                    className="flex items-center justify-between p-2.5 bg-slate-50 border border-slate-200 rounded-lg text-xs text-slate-700"
                  >
                    <div className="flex items-center gap-2 truncate">
                      <MdDescription className="w-4 h-4 text-emerald-600 shrink-0" />
                      <span className="truncate font-medium">{doc.name}</span>
                    </div>
                    <button
                      type="button"
                      onClick={() => handleRemoveDoc(doc.id)}
                      className="text-slate-400 hover:text-rose-500 p-1 transition-colors cursor-pointer"
                    >
                      <MdDeleteOutline className="w-4 h-4" />
                    </button>
                  </li>
                ))}
              </ul>
            ) : (
              <p className="text-xs text-slate-400 italic">No files attached yet.</p>
            )}
          </div>

          <div className="pt-4 border-t border-slate-100 flex items-center justify-end gap-3">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-100 rounded-lg transition-colors cursor-pointer"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-5 py-2 text-sm font-semibold text-white bg-emerald-600 hover:bg-emerald-700 rounded-lg shadow-sm transition-colors cursor-pointer"
            >
              Save Appointment
            </button>
          </div>
        </form>
      </aside>
    </div>
  );
};