import { CalendarClock, CheckCircle2 } from 'lucide-react';

const plans = [
  { name: 'CNC spindle inspection', frequency: 'Monthly', nextDue: '2026-08-26', status: 'On schedule', risk: 'Low' },
  { name: 'Boiler pressure verification', frequency: 'Weekly', nextDue: '2026-08-22', status: 'Due soon', risk: 'Medium' },
  { name: 'Robot lubrication cycle', frequency: 'Quarterly', nextDue: '2026-09-12', status: 'Planned', risk: 'Low' },
];

export default function PmPlansPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Preventive maintenance</p>
          <h1 className="text-3xl font-semibold text-white">PM Plans</h1>
        </div>
        <button className="rounded-xl bg-cyan-500 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-cyan-400">
          Generate PM schedule
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Schedule</p>
          <div className="mt-3 text-3xl font-semibold text-white">18</div>
          <p className="mt-1 text-sm text-slate-400">Active PM plans</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Due next 7 days</p>
          <div className="mt-3 text-3xl font-semibold text-white">04</div>
          <p className="mt-1 text-sm text-slate-400">Needs technician review</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Automation</p>
          <div className="mt-3 text-3xl font-semibold text-emerald-300">Live</div>
          <p className="mt-1 text-sm text-slate-400">Work orders auto-generated</p>
        </div>
      </div>

      <div className="grid gap-4 lg:grid-cols-3">
        {plans.map((plan) => (
          <div key={plan.name} className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-2 text-cyan-300">
                <CalendarClock className="h-4 w-4" />
                <span className="text-xs uppercase tracking-[0.18em]">Plan</span>
              </div>
              <div className={`rounded-full border px-2 py-1 text-[10px] uppercase tracking-[0.12em] ${plan.status === 'Due soon' ? 'border-amber-500/30 bg-amber-500/10 text-amber-200' : 'border-emerald-500/30 bg-emerald-500/10 text-emerald-200'}`}>
                {plan.status}
              </div>
            </div>

            <h2 className="text-lg font-semibold text-white">{plan.name}</h2>
            <div className="mt-4 space-y-2 text-sm text-slate-300">
              <div className="flex items-center justify-between">
                <span className="text-slate-400">Frequency</span>
                <span>{plan.frequency}</span>
              </div>
              <div className="flex items-center justify-between">
                <span className="text-slate-400">Next due</span>
                <span>{plan.nextDue}</span>
              </div>
              <div className="flex items-center justify-between">
                <span className="text-slate-400">Risk</span>
                <span>{plan.risk}</span>
              </div>
            </div>

            <button className="mt-5 inline-flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm font-medium text-white hover:border-cyan-500/40">
              <CheckCircle2 className="h-4 w-4 text-emerald-300" />
              View plan
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
