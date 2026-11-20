import { useEffect, useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import {
  MdClose,
  MdAttachFile,
  MdDeleteOutline,
  MdDescription,
  MdKeyboardArrowDown,
  MdCheck,
} from "react-icons/md";
import { AppointmentStatus } from "../../types/enums/appointmentStatus";
import type { Appointment } from "../../types/appointment";
import { useUserStore } from "../../store/useUser";
import { userApi } from "../../api/axios";

interface BackendDoctorDto {
  id: string;
  firstName?: string | null;
  lastName?: string | null;
  userName?: string | null;
  email?: string;
  specialty: string;
  roomNumber: number;
  avatarUrl?: string | null;
}

const appointmentDocumentSchema = z.object({
  id: z.string(),
  name: z.string(),
  type: z.string(),
  fileUrl: z.string(),
});

const createAppointmentSchema = z.object({
  doctorId: z.string().min(1, "Please select a doctor"),
  doctorName: z.string().min(1, "Doctor is required"),
  doctorSpecialty: z.string().min(1, "Specialty is required"),
  doctorAvatarUrl: z.string().url("Must be a valid URL").or(z.literal("")).optional(),
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

export const CreateAppointmentTab = ({
  isOpen,
  onClose,
  onSave,
}: CreateAppointmentTabProps) => {
  const user = useUserStore((state) => state.user);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const dropdownRef = useRef<HTMLDivElement | null>(null);

  const [doctors, setDoctors] = useState<BackendDoctorDto[]>([]);
  const [isLoadingDoctors, setIsLoadingDoctors] = useState(false);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

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
      doctorId: "",
      doctorName: "",
      doctorSpecialty: "",
      doctorAvatarUrl: "",
      date: "",
      time: "",
      room: 0,
      status: AppointmentStatus.Upcoming,
      documents: [],
    },
  });

  const documents = watch("documents");
  const selectedDoctorId = watch("doctorId");
  const selectedDoctorName = watch("doctorName");
  const roomValue = watch("room");

  useEffect(() => {
    if (!isOpen) return;

    const fetchDoctors = async () => {
      setIsLoadingDoctors(true);
      try {
        const response = await userApi.get<BackendDoctorDto[]>("/doctors");
        setDoctors(response.data);
      } catch (error) {
        console.error("Failed to fetch doctors:", error);
      } finally {
        setIsLoadingDoctors(false);
      }
    };

    fetchDoctors();
  }, [isOpen]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsDropdownOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  if (!isOpen) return null;

  const formatDoctorName = (doc: BackendDoctorDto) => {
    const fullName = [doc.firstName, doc.lastName].filter(Boolean).join(" ").trim();
    if (fullName) return `Dr. ${fullName}`;
    return doc.userName || doc.email || "";
  };

  const handleSelectDoctor = (doc: BackendDoctorDto) => {
    const formattedName = formatDoctorName(doc);

    setValue("doctorId", doc.id, { shouldValidate: true });
    setValue("doctorName", formattedName, { shouldValidate: true });
    setValue("doctorSpecialty", doc.specialty ?? "", { shouldValidate: true });
    setValue("room", doc.roomNumber ?? 0, { shouldValidate: true });
    setValue("doctorAvatarUrl", doc.avatarUrl || "", { shouldValidate: true });

    setIsDropdownOpen(false);
  };

  const handleFileUpload = (files: FileList | null) => {
    if (!files || files.length === 0) return;

    const newDocs = Array.from(files).map((file) => ({
      id: crypto.randomUUID(),
      name: file.name,
      type: file.type || "application/octet-stream",
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
      patientId: user?.id ?? "00000000-0000-0000-0000-000000000000",
      patientName: user?.userName || user?.email || "Unknown Patient",
      patientAvatarUrl: user?.avatarUrl ?? null,
      doctorId: data.doctorId,
      doctorName: data.doctorName,
      doctorSpecialty: data.doctorSpecialty,
      doctorAvatarUrl: data.doctorAvatarUrl || null,
      date: data.date,
      time: data.time,
      room: data.room,
      status: data.status,
      documents: data.documents.length > 0 ? data.documents : undefined,
    });

    reset();
    onClose();
  };

  const handleModalClose = () => {
    setIsDropdownOpen(false);
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div
        className="fixed inset-0 bg-slate-900/40 backdrop-blur-xs transition-opacity"
        onClick={handleModalClose}
      />

      <aside className="relative w-full max-w-lg max-h-[90vh] bg-white rounded-2xl shadow-2xl flex flex-col z-10 overflow-hidden">
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">New Appointment</h2>
            <p className="text-xs text-slate-400 mt-0.5">
              Fill in appointment and clinical details
            </p>
          </div>
          <button
            type="button"
            onClick={handleModalClose}
            className="p-2 text-slate-400 hover:text-slate-600 rounded-lg hover:bg-slate-100 transition-colors cursor-pointer"
          >
            <MdClose className="w-5 h-5" />
          </button>
        </div>

        <form
          onSubmit={handleSubmit(submitAppointment)}
          className="flex-1 overflow-y-auto p-6 space-y-4"
        >
          <div className="relative" ref={dropdownRef}>
            <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
              Doctor
            </label>
            <button
              type="button"
              onClick={() => setIsDropdownOpen((prev) => !prev)}
              className={`w-full px-3.5 py-2 text-sm border rounded-lg flex items-center justify-between bg-white text-left transition-all cursor-pointer ${
                errors.doctorId || errors.doctorName
                  ? "border-red-500 focus:ring-red-500/20"
                  : isDropdownOpen
                    ? "border-emerald-600 ring-2 ring-emerald-500/20"
                    : "border-slate-200 hover:border-slate-300"
              }`}
            >
              <span
                className={
                  selectedDoctorName ? "text-slate-800 font-medium" : "text-slate-400"
                }
              >
                {isLoadingDoctors
                  ? "Loading doctors..."
                  : selectedDoctorName || "Select a doctor"}
              </span>
              <MdKeyboardArrowDown
                className={`w-5 h-5 text-slate-400 transition-transform duration-150 ${
                  isDropdownOpen ? "rotate-180" : ""
                }`}
              />
            </button>

            {isDropdownOpen && (
              <ul
                role="listbox"
                className="absolute z-30 mt-1.5 w-full max-h-48 overflow-y-auto bg-white border border-slate-200 rounded-xl shadow-lg divide-y divide-slate-100"
              >
                {isLoadingDoctors ? (
                  <li className="px-4 py-3 text-xs text-slate-400 text-center">
                    Loading doctors...
                  </li>
                ) : doctors.length === 0 ? (
                  <li className="px-4 py-3 text-xs text-slate-400 text-center">
                    No doctors available
                  </li>
                ) : (
                  doctors.map((doc) => {
                    const isSelected = doc.id === selectedDoctorId;
                    return (
                      <li
                        key={doc.id}
                        role="option"
                        aria-selected={isSelected}
                        onClick={() => handleSelectDoctor(doc)}
                        className={`px-3.5 py-2.5 flex items-center justify-between cursor-pointer transition-colors ${
                          isSelected
                            ? "bg-emerald-50/80 text-emerald-900"
                            : "hover:bg-slate-50 text-slate-700"
                        }`}
                      >
                        <div className="min-w-0 pr-2">
                          <p className="text-sm font-medium truncate">
                            {formatDoctorName(doc)}
                          </p>
                          <p className="text-xs text-slate-400 truncate">
                            {doc.specialty} • Room {doc.roomNumber}
                          </p>
                        </div>
                        {isSelected && (
                          <MdCheck className="w-4 h-4 text-emerald-600 shrink-0" />
                        )}
                      </li>
                    );
                  })
                )}
              </ul>
            )}

            {(errors.doctorId || errors.doctorName) && (
              <p className="text-red-500 text-xs font-medium mt-1">
                {errors.doctorId?.message || errors.doctorName?.message}
              </p>
            )}
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Specialty
              </label>
              <input
                readOnly
                disabled
                tabIndex={-1}
                placeholder="Auto-filled"
                className="w-full px-3.5 py-2 text-sm border border-slate-200 rounded-lg bg-slate-50 text-slate-500 cursor-not-allowed pointer-events-none select-none focus:outline-none"
                {...register("doctorSpecialty")}
              />
              {errors.doctorSpecialty && (
                <p className="text-red-500 text-xs font-medium mt-1">
                  {errors.doctorSpecialty.message}
                </p>
              )}
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-600 uppercase mb-1">
                Room Number
              </label>
              <input
                readOnly
                disabled
                tabIndex={-1}
                type="text"
                value={roomValue > 0 ? roomValue : ""}
                placeholder="Auto-filled"
                className="w-full px-3.5 py-2 text-sm border border-slate-200 rounded-lg bg-slate-50 text-slate-500 cursor-not-allowed pointer-events-none select-none focus:outline-none"
              />
              {errors.room && (
                <p className="text-red-500 text-xs font-medium mt-1">
                  {errors.room.message}
                </p>
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
                <p className="text-red-500 text-xs font-medium mt-1">
                  {errors.date.message}
                </p>
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
                <p className="text-red-500 text-xs font-medium mt-1">
                  {errors.time.message}
                </p>
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
              onClick={handleModalClose}
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