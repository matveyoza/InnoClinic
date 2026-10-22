import { useState } from "react";
import { 
    FaMapMarkerAlt, 
    FaCalendarAlt, 
    FaClock, 
    FaFileAlt, 
    FaUser, 
    FaTimes, 
    FaExternalLinkAlt 
} from "react-icons/fa";
import { AppointmentStatus } from "../../types/appointment";
import type { Appointment } from "../../types/appointment";
import type { AppointmentDocument } from "../../types/appointmentDocument";

interface AppointmentCardProps {
    appointment: Appointment;
}

const STATUS_STYLES: Record<AppointmentStatus, string> = {
    [AppointmentStatus.Upcoming]: 'bg-emerald-50 text-emerald-700 border-emerald-200',
    [AppointmentStatus.Completed]: 'bg-blue-50 text-blue-700 border-blue-200',
    [AppointmentStatus.Cancelled]: 'bg-slate-100 text-slate-500 border-slate-200',
};

export const AppointmentCard = ({ appointment }: AppointmentCardProps) => {
    const [selectedDoc, setSelectedDoc] = useState<AppointmentDocument | null>(null);

    const StatusBadge = STATUS_STYLES[appointment.status] ?? 'bg-slate-50 text-slate-700 border-slate-200';

    return (
        <div className="bg-white rounded-xl border border-slate-200 shadow-sm hover:shadow-md transition-all p-5 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
            <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-full bg-emerald-100 text-emerald-700 font-bold flex items-center justify-center text-lg shrink-0 overflow-hidden">
                    {appointment.doctorAvatarUrl ? (
                        <img
                            src={appointment.doctorAvatarUrl}
                            alt={appointment.doctorName}
                            className="w-12 h-12 rounded-full object-cover"
                        />
                    ) : (
                        <FaUser className="w-5 h-5 text-emerald-600" />
                    )}
                </div>
                <div>
                    <h3 className="font-semibold text-slate-800 text-base">{appointment.doctorName}</h3>
                    <p className="text-emerald-600 text-xs font-medium">{appointment.doctorSpecialty}</p>
                    <p className="text-slate-400 text-xs mt-1 flex items-center gap-1.5">
                        <FaMapMarkerAlt className="w-3 h-3 text-slate-400 shrink-0" />
                        <span>{appointment.location}</span>
                    </p>
                </div>
            </div>

            <div className="flex items-center gap-3 bg-slate-50 px-4 py-2.5 rounded-lg border border-slate-100">
                <div className="flex items-center gap-2">
                    <FaCalendarAlt className="w-3.5 h-3.5 text-emerald-600 shrink-0" />
                    <div className="text-left">
                        <span className="text-[10px] font-semibold text-slate-400 block uppercase leading-none mb-0.5">Date</span>
                        <span className="text-xs font-bold text-slate-800">{appointment.date}</span>
                    </div>
                </div>

                <div className="h-7 w-px bg-slate-200 mx-1" />

                <div className="flex items-center gap-2">
                    <FaClock className="w-3.5 h-3.5 text-emerald-600 shrink-0" />
                    <div className="text-left">
                        <span className="text-[10px] font-semibold text-slate-400 block uppercase leading-none mb-0.5">Time</span>
                        <span className="text-xs font-bold text-emerald-600">{appointment.time}</span>
                    </div>
                </div>
            </div>

            <div className="flex flex-col md:items-end gap-2 w-full md:w-auto">
                <span className={`text-xs px-2.5 py-1 rounded-full border font-semibold w-fit ${StatusBadge}`}>
                    {appointment.status}
                </span>

                {appointment.documents && appointment.documents.length > 0 && (
                    <div className="flex items-center gap-1.5 mt-1">
                        <span className="text-xs text-slate-400 mr-1">Docs:</span>
                        {appointment.documents.map((doc) => (
                            <button
                                key={doc.id}
                                onClick={() => setSelectedDoc(doc)}
                                className="inline-flex items-center gap-1.5 text-xs font-medium text-emerald-700 bg-emerald-50 hover:bg-emerald-100 border border-emerald-200/60 px-2.5 py-1 rounded-lg cursor-pointer transition-colors"
                            >
                                <FaFileAlt className="w-3 h-3 text-emerald-600" />
                                <span>{doc.name}</span>
                            </button>
                        ))}
                    </div>
                )}
            </div>

            {selectedDoc && (
                <div className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm z-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded-2xl max-w-md w-full p-6 shadow-xl border border-slate-100 relative">
                        <button
                            onClick={() => setSelectedDoc(null)}
                            className="absolute top-4 right-4 p-1 text-slate-400 hover:text-slate-600 rounded-lg transition-colors cursor-pointer"
                        >
                            <FaTimes className="w-4 h-4" />
                        </button>

                        <div className="flex items-center gap-3 mb-4">
                            <div className="p-2.5 bg-emerald-50 text-emerald-600 rounded-xl">
                                <FaFileAlt className="w-5 h-5" />
                            </div>
                            <div>
                                <h4 className="text-base font-bold text-slate-800">{selectedDoc.name}</h4>
                                <p className="text-xs text-slate-400">{selectedDoc.type} Document</p>
                            </div>
                        </div>

                        <div className="bg-slate-50 p-8 rounded-xl text-center text-slate-400 text-sm mb-6 border border-slate-200 flex flex-col items-center justify-center gap-2">
                            <FaFileAlt className="w-8 h-8 text-slate-300" />
                            <span>Document preview for <strong className="text-slate-600">{selectedDoc.fileUrl}</strong></span>
                        </div>

                        <div className="flex justify-end gap-2">
                            <a
                                href={selectedDoc.fileUrl}
                                target="_blank"
                                rel="noopener noreferrer"
                                className="inline-flex items-center gap-1.5 px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white rounded-xl text-sm font-medium transition-colors"
                            >
                                <FaExternalLinkAlt className="w-3.5 h-3.5" />
                                Open File
                            </a>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};