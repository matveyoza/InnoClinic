import type { AppointmentDocument } from '../types/appointmentDocument';
import { AppointmentStatus } from '../types/enums/appointmentStatus';

export interface Appointment {
    id: string;
    doctorName: string;
    doctorSpecialty: string;
    doctorAvatarUrl?: string;
    date: string;
    time: string;
    status: AppointmentStatus;
    room: number;
    documents?: AppointmentDocument[];
}