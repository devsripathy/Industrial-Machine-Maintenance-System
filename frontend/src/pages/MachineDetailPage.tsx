import { useQuery } from '@tanstack/react-query';
import { ArrowLeft, Calendar, Cpu, Factory, MapPin, QrCode, ShieldAlert, Wrench } from 'lucide-react';
import { Link, useParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { fetchMachineById } from '../lib/api';

export default function MachineDetailPage() {
  const { id } = useParams();
  const { accessToken } = useAuth();

  const { data: machine, isLoading, error } = useQuery({
    queryKey: ['machine', id],
    queryFn: () => fetchMachineById(accessToken ?? '', Number(id)),
    enabled: Boolean(accessToken && id),
  });

  if (isLoading) {
    return <div className="text-sm text-slate-400">Loading machine details...</div>;
  }

  if (error || !machine) {
    return <div className="rounded-2xl border border-rose-500/30 bg-rose-500/10 p-4 text-rose-200">Unable to load machine details.</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-3">
        <Link to="/machines" className="flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-slate-300 hover:border-cyan-500/40">
          <ArrowLeft className="h-4 w-4" />
          Back to assets
        </Link>
      </div>

      <div className="grid gap-6 xl:grid-cols-[1.6fr_0.9fr]">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="flex flex-col gap-3 md:flex-row md:items-start md:justify-between">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Asset detail</p>
              <h1 className="mt-2 text-3xl font-semibold text-white">{machine.name}</h1>
              <p className="mt-2 text-sm text-slate-400">{machine.code} • {machine.machineTypeName}</p>
            </div>
            <div className="rounded-full border border-emerald-500/30 bg-emerald-500/10 px-3 py-1.5 text-sm font-medium text-emerald-200">
              {machine.status}
            </div>
          </div>

          <div className="mt-6 grid gap-4 md:grid-cols-2 xl:grid-cols-4">
            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="text-xs uppercase tracking-[0.12em] text-slate-500">Criticality</div>
              <div className="mt-2 text-lg font-semibold text-white">{machine.criticality}</div>
            </div>
            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="text-xs uppercase tracking-[0.12em] text-slate-500">Location</div>
              <div className="mt-2 text-lg font-semibold text-white">{machine.location}</div>
            </div>
            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="text-xs uppercase tracking-[0.12em] text-slate-500">Model</div>
              <div className="mt-2 text-lg font-semibold text-white">{machine.model}</div>
            </div>
            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="text-xs uppercase tracking-[0.12em] text-slate-500">Serial</div>
              <div className="mt-2 text-lg font-semibold text-white">{machine.serialNumber}</div>
            </div>
          </div>

          <div className="mt-6 grid gap-6 lg:grid-cols-2">
            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="mb-3 flex items-center gap-2 text-cyan-300">
                <Factory className="h-4 w-4" />
                <span className="text-sm font-medium">Specifications</span>
              </div>
              <pre className="whitespace-pre-wrap text-sm text-slate-300">{machine.specificationsJson}</pre>
            </div>

            <div className="rounded-xl border border-slate-800 bg-slate-950/70 p-4">
              <div className="mb-3 flex items-center gap-2 text-violet-300">
                <Wrench className="h-4 w-4" />
                <span className="text-sm font-medium">Dependencies</span>
              </div>
              {machine.dependencies.length ? (
                <div className="space-y-2 text-sm text-slate-300">
                  {machine.dependencies.map((dep) => (
                    <div key={`${dep.machineId}-${dep.dependsOnMachineId}`} className="rounded-lg border border-slate-800 bg-slate-900 p-2">
                      {dep.dependsOnMachineName} ({dep.dependsOnMachineCode})
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-sm text-slate-400">No active dependencies.</p>
              )}
            </div>
          </div>
        </div>

        <div className="space-y-6">
          <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
            <div className="mb-4 flex items-center gap-2 text-cyan-300">
              <QrCode className="h-4 w-4" />
              <span className="text-sm font-medium">QR / Asset tag</span>
            </div>
            <div className="flex h-48 items-center justify-center rounded-2xl border border-dashed border-slate-700 bg-slate-950/70 text-center text-sm text-slate-300">
              <div>
                <div className="mb-2 font-mono text-lg tracking-[0.2em]">{machine.code}</div>
                <div className="text-xs text-slate-500">{machine.qrCodeData}</div>
              </div>
            </div>
          </div>

          <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
            <div className="mb-4 flex items-center gap-2 text-amber-300">
              <ShieldAlert className="h-4 w-4" />
              <span className="text-sm font-medium">Asset metadata</span>
            </div>
            <div className="space-y-3 text-sm text-slate-300">
              <div className="flex items-center justify-between gap-2 rounded-lg border border-slate-800 bg-slate-950/70 p-2">
                <span className="flex items-center gap-2"><Calendar className="h-3.5 w-3.5 text-slate-500" /> Install date</span>
                <span>{machine.installDate ? new Date(machine.installDate).toLocaleDateString() : 'N/A'}</span>
              </div>
              <div className="flex items-center justify-between gap-2 rounded-lg border border-slate-800 bg-slate-950/70 p-2">
                <span className="flex items-center gap-2"><MapPin className="h-3.5 w-3.5 text-slate-500" /> Location</span>
                <span>{machine.location}</span>
              </div>
              <div className="flex items-center justify-between gap-2 rounded-lg border border-slate-800 bg-slate-950/70 p-2">
                <span className="flex items-center gap-2"><Cpu className="h-3.5 w-3.5 text-slate-500" /> Type</span>
                <span>{machine.machineTypeName}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
