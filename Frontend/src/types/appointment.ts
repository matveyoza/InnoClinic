export interface AppointmentDocument {
    id: string;
    name: string;
    type: string;
    url: string;
}

export interface Appointment {
    id: string;
    doctorName: string;
    doctorSpecialty: string;
    doctorAvatar?: string;
    date: string;
    time: string;
    status: 'Upcoming' | 'Completed' | 'Cancelled';
    location: string;
    documents?: AppointmentDocument[];
}