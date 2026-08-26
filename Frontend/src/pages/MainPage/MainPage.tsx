import { Layout } from '../../components/Layout/Layout';

export const MainPage = () => {
    return (
        <Layout>
            <div>
                <h1 className="text-2xl font-bold text-slate-900">Dashboard</h1>
            </div>
                
            <div className="mt-4.5 bg-white rounded-xl border border-slate-200 shadow-sm p-6">
                <div className="flex justify-between items-center pb-4 border-b border-slate-100">
                    <h2 className="text-lg font-bold text-slate-800">Upcoming Appointments</h2>
      
                    <div className="text-right">
                        <span className="text-xs font-semibold tracking-wider text-slate-400 uppercase block">
                            Today's Visits
                        </span>
                        <p className="text-2xl font-extrabold text-slate-900 leading-tight">4</p>
                    </div>
                </div>

                <div className="text-sm text-slate-500 py-12 text-center rounded-lg">
                    No upcoming appointments scheduled for today.
                </div>
            </div>
        </Layout>
    );
};