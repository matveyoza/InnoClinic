import { useNavigate } from 'react-router-dom';
import { AppointmentCard } from '../../components/Appointments/AppointmentCard';
import { Layout } from '../../components/Layout/Layout';
import { useUserStore } from '../../store/useUserStore';
import type { Appointment } from '../../types/appointment';
import { AppointmentStatus } from '../../types/enums/appointmentStatus';

const mockAppointments: Appointment[] = [
    {
        id: '1',
        doctorName: 'Dr. Sarah Jenkins',
        doctorSpecialty: 'Cardiologist',
        date: 'Oct 24, 2026',
        time: '10:30 AM',
        status: AppointmentStatus.Upcoming,
        cabinet: 'Cabinet 204',
        documents: [
            { id: 'doc1', name: 'ECG_Report.pdf', type: 'PDF', fileUrl: '/docs/ecg.pdf' },
            { id: 'doc2', name: 'Prescription.pdf', type: 'PDF', fileUrl: '/docs/rx.pdf' },
        ],
    },
    {
        id: '2',
        doctorName: 'Dr. Michael Chen',
        doctorSpecialty: 'General Practitioner',
        date: 'Oct 28, 2026',
        time: '02:15 PM',
        status: AppointmentStatus.Upcoming,
        cabinet: 'Cabinet 101',
    },
    {
        id: '3',
        doctorName: 'Dr. Emily Vance',
        doctorSpecialty: 'Dermatologist',
        date: 'Sep 15, 2026',
        time: '11:00 AM',
        status: AppointmentStatus.Completed,
        cabinet: 'Cabinet 305',
        documents: [
            { id: 'doc3', name: 'Skin_Analysis.pdf', type: 'PDF', fileUrl: '/docs/skin.pdf' },
        ],
    },
];

export const MainPage = () => {
    const user = useUserStore(state => state.user);
    const navigate = useNavigate();
    const upcomingCount = mockAppointments.filter(app => app.status === AppointmentStatus.Upcoming).length;

    return (
        <Layout user={user}>
            <div>
                <h1 className="text-2xl font-bold text-slate-900">Dashboard</h1>
            </div>

            <div className="mt-4.5 bg-white rounded-xl border border-slate-200 shadow-sm p-6">
                <div className="flex justify-between items-center pb-4 border-b border-slate-100">
                    <h2 className="text-lg font-bold text-slate-800">My Appointments</h2>
                    
                    <button
                        type="button"
                        onClick={() => navigate('/appointments/create')}
                        className="inline-flex items-center gap-2 px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white text-sm font-semibold rounded-lg shadow-sm transition-colors cursor-pointer"
                    >
                        Create Appointment
                    </button>

                    <div className="text-right">
                        <span className="text-xs font-semibold tracking-wider text-slate-400 uppercase block">
                            Upcoming Visits
                        </span>
                        <p className="text-2xl font-extrabold text-slate-900 leading-tight">
                            {upcomingCount}
                        </p>
                    </div>
                </div>

                <div className="mt-6 space-y-4">
                    {mockAppointments.length > 0 ? (
                        mockAppointments.map((appointment) => (
                            <AppointmentCard key={appointment.id} appointment={appointment} />
                        ))
                    ) : (
                        <div className="text-sm text-slate-500 py-12 text-center rounded-lg">
                            No appointments scheduled.
                        </div>
                    )}
                </div>
            </div>
        </Layout>
    );
};