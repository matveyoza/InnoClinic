import type { AppointmentDocument } from '../types/appointmentDocument';

export const enum AppointmentStatus {
    Upcoming = 'Upcoming',
    Completed = 'Completed',
    Cancelled = 'Cancelled',
}

export interface Appointment {
    id: string;
    doctorName: string;
    doctorSpecialty: string;
    doctorAvatarUrl?: string;
    date: string;
    time: string;
    status: AppointmentStatus;
    location: string;
    documents?: AppointmentDocument[];
}