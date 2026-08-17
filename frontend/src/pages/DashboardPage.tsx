import { Activity, AlertTriangle, ArrowUpRight, Factory, Wrench } from 'lucide-react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { fetchMachines } from '../lib/api';

const stats = [
  { label: 'Assets online', value: '94%', change: '+2.4%', tone: 'emerald' },
  { label: 'Downtime hours', value: '8.6h', change: '-14%', tone: 'cyan' },
  { label: 'Open work orders', value: '14', change: '+3', tone: 'amber' },
  { label: 'MTTR', value: '7.2h', change: '-18%', tone: 'violet' },
];

export default function DashboardPage() {
  const { accessToken } = useAuth();

  const { data: machines = [], isLoading } = useQuery({
    queryKey: ['machines'],
    queryFn: () => fetchMachines(accessToken ?? ''),
    enabled: Boolean(accessToken),
  });

  const criticalMachines = machines.filter((machine) => machine.criticalityValue >= 3).slice(0, 4);

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-2 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Overview</p>
          <h1 className="text-3xl font-semibold text-white">Plant performance dashboard</h1>
        </div>
        <button className="rounded-xl border border-cyan-500/40 bg-cyan-500/10 px-4 py-2 text-sm font-medium text-cyan-200 transition hover:bg-cyan-500/20">
          Generate shift report
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        {stats.map((stat) => (
          <div key={stat.label} className="rounded-2xl border border-slate-800 bg-slate-900 p-4 shadow-lg shadow-slate-950/20">
            <div className="mb-5 flex items-center justify-between text-slate-400">
              <span className="text-sm">{stat.label}</span>
              <ArrowUpRight className="h-4 w-4 text-emerald-300" />
            </div>
            <div className="flex items-end justify-between">
              <span className="text-3xl font-semibold text-white">{stat.value}</span>
              <span className="text-sm text-emerald-300">{stat.change}</span>
            </div>
          </div>
        ))}
      </div>

      <div className="grid gap-6 xl:grid-cols-[1.5fr_1fr]">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="mb-4 flex items-center justify-between">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Critical assets</p>
              <h2 className="mt-2 text-xl font-semibold text-white">Priority fleet watchlist</h2>
            </div>
            <Factory className="h-5 w-5 text-cyan-300" />
          </div>

          <div className="space-y-3">
            {isLoading ? (
              <div className="text-sm text-slate-400">Loading machine health...</div>
            ) : criticalMachines.length ? (
              criticalMachines.map((machine) => (
                <Link
                  key={machine.id}
                  to={`/machines/${machine.id}`}
                  className="flex items-center justify-between rounded-xl border border-slate-800 bg-slate-950/70 p-3 transition hover:border-cyan-500/40 hover:bg-slate-950"
                >
                  <div>
                    <div className="flex items-center gap-2">
                      <span className="text-sm font-medium text-white">{machine.name}</span>
                      <span className="rounded-full border border-amber-500/40 bg-amber-500/10 px-2 py-0.5 text-[10px] uppercase tracking-wider text-amber-300">
                        {machine.criticality}
                      </span>
                    </div>
                    <p className="mt-1 text-xs text-slate-400">{machine.code} • {machine.location}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-xs uppercase tracking-[0.2em] text-slate-500">Status</p>
                    <p className="text-sm font-medium text-emerald-300">{machine.status}</p>
                  </div>
                </Link>
              ))
            ) : (
              <p className="text-sm text-slate-400">No critical machines flagged.</p>
            )}
          </div>
        </div>

        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="mb-4 flex items-center justify-between">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Action center</p>
              <h2 className="mt-2 text-xl font-semibold text-white">Operational alerts</h2>
            </div>
            <AlertTriangle className="h-5 w-5 text-amber-300" />
          </div>

          <div className="space-y-3 text-sm text-slate-300">
            <div className="rounded-xl border border-amber-500/30 bg-amber-500/10 p-3">
              <div className="font-medium text-amber-200">Boiler inspection required</div>
              <p className="mt-1 text-xs text-amber-100/80">Due in 2 days • Utility Block - Powerhouse</p>
            </div>
            <div className="rounded-xl border border-cyan-500/25 bg-cyan-500/10 p-3">
              <div className="font-medium text-cyan-200">PM plan review</div>
              <p className="mt-1 text-xs text-cyan-100/80">3 preventive tasks due this shift</p>
            </div>
            <div className="rounded-xl border border-slate-700 bg-slate-950/70 p-3">
              <div className="font-medium text-white">Inventory check</div>
              <p className="mt-1 text-xs text-slate-300">7 spare parts below reorder threshold</p>
            </div>
          </div>
        </div>
      </div>

      <div className="grid gap-6 xl:grid-cols-[1.5fr_1fr]">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="mb-4 flex items-center justify-between">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Maintenance</p>
              <h2 className="mt-2 text-xl font-semibold text-white">Overdue work orders</h2>
            </div>
            <Wrench className="h-5 w-5 text-violet-300" />
          </div>

          <div className="space-y-3">
            {[
              { title: 'Emergency inspection', machine: 'CNC-01', due: '2 days overdue' },
              { title: 'Valve calibration', machine: 'BRL-01', due: '5 days overdue' },
              { title: 'Conveyor belt drive check', machine: 'ROB-01', due: '1 day overdue' },
            ].map((job) => (
              <div key={job.title} className="flex items-center justify-between rounded-xl border border-slate-800 bg-slate-950/70 p-3">
                <div>
                  <div className="font-medium text-white">{job.title}</div>
                  <div className="text-xs text-slate-400">{job.machine}</div>
                </div>
                <div className="rounded-full bg-rose-500/10 px-2 py-1 text-xs font-medium text-rose-200">{job.due}</div>
              </div>
            ))}
          </div>
        </div>

        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="mb-4 flex items-center justify-between">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Health</p>
              <h2 className="mt-2 text-xl font-semibold text-white">Asset trends</h2>
            </div>
            <Activity className="h-5 w-5 text-emerald-300" />
          </div>

          <div className="space-y-3">
            {[{ label: 'Availability', value: '98.7%' }, { label: 'Planned maintenance rate', value: '63%' }, { label: 'Condition alerts', value: '09' }].map((item) => (
              <div key={item.label} className="rounded-xl border border-slate-800 bg-slate-950/70 p-3">
                <div className="flex items-center justify-between text-sm text-slate-300">
                  <span>{item.label}</span>
                  <span className="font-semibold text-white">{item.value}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
