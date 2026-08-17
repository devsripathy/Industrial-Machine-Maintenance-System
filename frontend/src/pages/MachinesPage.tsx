import { useQuery } from '@tanstack/react-query';
import { ArrowRight, Cpu, Search, Wrench } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { fetchMachines } from '../lib/api';

export default function MachinesPage() {
  const { accessToken } = useAuth();

  const { data: machines = [], isLoading } = useQuery({
    queryKey: ['machines'],
    queryFn: () => fetchMachines(accessToken ?? ''),
    enabled: Boolean(accessToken),
  });

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Asset registry</p>
          <h1 className="text-3xl font-semibold text-white">Machines</h1>
        </div>
        <div className="flex items-center gap-3">
          <div className="flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-slate-300">
            <Search className="h-4 w-4 text-slate-500" />
            <input
              className="w-48 bg-transparent text-white outline-none placeholder:text-slate-500"
              placeholder="Search assets"
            />
          </div>
          <button className="rounded-xl bg-cyan-500 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-cyan-400">
            Add machine
          </button>
        </div>
      </div>

      <div className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
        {isLoading ? (
          <div className="text-sm text-slate-400">Loading machine registry...</div>
        ) : (
          <div className="space-y-3">
            {machines.map((machine) => (
              <div
                key={machine.id}
                className="flex flex-col gap-4 rounded-xl border border-slate-800 bg-slate-950/70 p-4 md:flex-row md:items-center md:justify-between"
              >
                <div className="flex items-start gap-4">
                  <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-cyan-500/10 text-cyan-300 ring-1 ring-cyan-500/30">
                    <Cpu className="h-5 w-5" />
                  </div>
                  <div>
                    <div className="flex items-center gap-2">
                      <h2 className="text-lg font-semibold text-white">{machine.name}</h2>
                      <span className="rounded-full border border-violet-500/30 bg-violet-500/10 px-2 py-0.5 text-[10px] uppercase tracking-wider text-violet-200">
                        {machine.criticality}
                      </span>
                    </div>
                    <p className="mt-1 text-sm text-slate-400">{machine.code} • {machine.machineTypeName}</p>
                    <p className="mt-1 text-xs text-slate-500">{machine.location}</p>
                  </div>
                </div>

                <div className="flex flex-wrap items-center gap-3 text-sm text-slate-300 md:justify-end">
                  <div className="rounded-full border border-slate-700 bg-slate-900 px-3 py-1.5 text-xs uppercase tracking-[0.12em] text-cyan-200">
                    {machine.status}
                  </div>
                  <div className="flex items-center gap-2 text-xs text-slate-400">
                    <Wrench className="h-3.5 w-3.5" />
                    {machine.dependencies.length} dependencies
                  </div>
                  <Link
                    to={`/machines/${machine.id}`}
                    className="inline-flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-900 px-3 py-2 font-medium text-white transition hover:border-cyan-500/40 hover:text-cyan-200"
                  >
                    Open detail
                    <ArrowRight className="h-4 w-4" />
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
