import { useEffect, useState } from 'react';
import { appointmentApi } from '../../api/axios';
import { AppointmentCard } from '../../components/Appointments/AppointmentCard';
import { CreateAppointmentTab } from '../../components/Appointments/CreateAppointmentTab';
import { Layout } from '../../components/Layout/Layout';
import { useUserStore } from '../../store/useUserStore';
import type { Appointment } from '../../types/appointment';
import { AppointmentStatus } from '../../types/enums/appointmentStatus';

export const MainPage = () => {
  const user = useUserStore((state) => state.user);
  const hasRole = useUserStore((state) => state.hasRole);

  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isCreateOpen, setIsCreateOpen] = useState<boolean>(false);

  const isDoctor = hasRole('Doctor');
  const isAdmin = hasRole('Admin');
  const canCreate = hasRole(['Patient', 'Admin']);

  const fetchAppointments = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await appointmentApi.get<Appointment[]>('');
      setAppointments(response.data);
    } catch {
      setError('Failed to load appointments. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAppointments();
  }, []);

  const handleSaveAppointment = async (newAppointment: Omit<Appointment, 'id'>) => {
    try {
      await appointmentApi.post('', newAppointment);
      await fetchAppointments();
    } catch {
      setError('Failed to create appointment.');
    }
  };

  const upcomingCount = appointments.filter(
    (app) => app.status === AppointmentStatus.Upcoming
  ).length;

  const pageTitle = isAdmin
    ? 'Administration Dashboard'
    : isDoctor
      ? 'Doctor Schedule'
      : 'Dashboard';

  const sectionTitle = isAdmin
    ? 'All Clinic Appointments'
    : isDoctor
      ? 'My Consultations'
      : 'My Appointments';

  const countLabel = isDoctor ? 'Upcoming Patients' : 'Upcoming Visits';

  return (
    <Layout user={user}>
      <div>
        <h1 className="text-2xl font-bold text-slate-900">{pageTitle}</h1>
      </div>

      <div className="mt-4.5 bg-white rounded-xl border border-slate-200 shadow-sm p-6">
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center pb-4 border-b border-slate-100 gap-4">
          <h2 className="text-lg font-bold text-slate-800">{sectionTitle}</h2>

          <div className="flex items-center gap-6">
            {canCreate && (
              <button
                type="button"
                onClick={() => setIsCreateOpen(true)}
                className="inline-flex items-center gap-2 px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white text-sm font-semibold rounded-lg shadow-sm transition-colors cursor-pointer"
              >
                Create Appointment
              </button>
            )}

            <div className="text-right">
              <span className="text-xs font-semibold tracking-wider text-slate-400 uppercase block">
                {countLabel}
              </span>
              <p className="text-2xl font-extrabold text-slate-900 leading-tight">
                {upcomingCount}
              </p>
            </div>
          </div>
        </div>

        <div className="mt-6 space-y-4">
          {loading ? (
            <div className="py-12 text-center text-slate-400 font-medium animate-pulse">
              Loading appointments...
            </div>
          ) : error ? (
            <div className="py-6 text-center text-red-500 bg-red-50 rounded-lg text-sm border border-red-100">
              {error}
            </div>
          ) : appointments.length > 0 ? (
            appointments.map((appointment) => (
              <AppointmentCard key={appointment.id} appointment={appointment} />
            ))
          ) : (
            <div className="text-sm text-slate-500 py-12 text-center rounded-lg bg-slate-50 border border-dashed border-slate-200">
              {isDoctor
                ? 'No consultations scheduled for your patients.'
                : 'No appointments scheduled.'}
            </div>
          )}
        </div>
      </div>

      <CreateAppointmentTab
        isOpen={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        onSave={handleSaveAppointment}
      />
    </Layout>
  );
};