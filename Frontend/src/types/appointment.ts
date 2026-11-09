import type { AppointmentDocument } from '../types/appointmentDocument';
import { AppointmentStatus } from '../types/enums/appointmentStatus';

export interface Appointment {
    id: string;
    patientId: string;
    patientName: string;
    doctorId: string;
    doctorName: string;
    doctorSpecialty: string;
    doctorAvatarUrl?: string | null;
    date: string;
    time: string;
    room: number;
    status: AppointmentStatus;
    documents?: AppointmentDocument[];
    createdAtUtc?: string; 
}