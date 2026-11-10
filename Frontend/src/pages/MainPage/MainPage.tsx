import { useState } from 'react';
import { useLoaderData, useRevalidator, useNavigation } from 'react-router-dom';
import { appointmentApi } from '../../api/axios';
import { AppointmentCard } from '../../components/Appointments/AppointmentCard';
import { CreateAppointmentTab } from '../../components/Appointments/CreateAppointmentTab';
import { Layout } from '../../components/Layout/Layout';
import { useUser } from '../../store/useUser';
import type { Appointment } from '../../types/appointment';
import { AppointmentStatus } from '../../types/enums/appointmentStatus';

export const mainPageLoader = async (): Promise<Appointment[]> => {
  const response = await appointmentApi.get<Appointment[]>('');
  return response.data;
};

export const MainPage = () => {
  const user = useUser((state) => state.user);
  const hasRole = useUser((state) => state.hasRole);

  const appointments = useLoaderData() as Appointment[];
  const revalidator = useRevalidator();
  const navigation = useNavigation();

  const [isCreateOpen, setIsCreateOpen] = useState<boolean>(false);
  const [actionError, setActionError] = useState<string | null>(null);

  const isRevalidating = revalidator.state === 'loading' || navigation.state === 'loading';

  const isDoctor = hasRole('Doctor');
  const isAdmin = hasRole('Admin');
  const canCreate = hasRole(['Patient', 'Admin']);

  const handleSaveAppointment = async (newAppointment: Omit<Appointment, 'id'>) => {
    setActionError(null);
    try {
      await appointmentApi.post('', newAppointment);
      revalidator.revalidate();
    } catch {
      setActionError('Failed to create appointment.');
    }
  };

  const upcomingCount = appointments.filter(
    (app) =>
      app.status === AppointmentStatus.Upcoming ||
      (app.status as unknown) === 0
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

        {actionError && (
          <div className="mt-4 py-3 px-4 text-center text-red-600 bg-red-50 rounded-lg text-sm border border-red-200">
            {actionError}
          </div>
        )}

        <div className="mt-6 space-y-4">
          {isRevalidating ? (
            <div className="py-12 text-center text-slate-400 font-medium animate-pulse">
              Refreshing appointments...
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