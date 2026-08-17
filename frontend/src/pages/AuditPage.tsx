import { FileText, ShieldCheck, TriangleAlert } from 'lucide-react';

const audits = [
  { action: 'Updated asset', user: 'admin', timestamp: '2026-08-17 05:15', entity: 'Machine#CNC-01', severity: 'Info' },
  { action: 'Approved work order', user: 'maintenance-lead', timestamp: '2026-08-17 04:42', entity: 'WO-102', severity: 'Important' },
  { action: 'Created spare part request', user: 'technician', timestamp: '2026-08-17 04:15', entity: 'Inventory#DB-195', severity: 'Info' },
];

export default function AuditPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Security & compliance</p>
          <h1 className="text-3xl font-semibold text-white">Audit Logs</h1>
        </div>
        <button className="rounded-xl border border-slate-700 bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:border-cyan-500/40">
          Export activity
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Events today</p>
          <div className="mt-3 text-3xl font-semibold text-white">128</div>
          <p className="mt-1 text-sm text-slate-400">Activity across the plant</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">IC review</p>
          <div className="mt-3 text-3xl font-semibold text-cyan-300">24</div>
          <p className="mt-1 text-sm text-slate-400">Approvals requiring attention</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Exceptions</p>
          <div className="mt-3 text-3xl font-semibold text-amber-300">03</div>
          <p className="mt-1 text-sm text-slate-400">Escalations this shift</p>
        </div>
      </div>

      <div className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
        <div className="mb-4 flex items-center gap-2 text-sm text-slate-300">
          <ShieldCheck className="h-4 w-4 text-emerald-300" />
          Latest operational activity
        </div>

        <div className="space-y-3">
          {audits.map((audit) => (
            <div key={`${audit.entity}-${audit.timestamp}`} className="flex flex-col gap-2 rounded-xl border border-slate-800 bg-slate-950/70 p-3 md:flex-row md:items-center md:justify-between">
              <div className="flex items-center gap-3">
                <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-cyan-500/10 text-cyan-300">
                  <FileText className="h-4 w-4" />
                </div>
                <div>
                  <div className="font-medium text-white">{audit.action}</div>
                  <div className="text-xs text-slate-400">{audit.entity}</div>
                </div>
              </div>

              <div className="flex items-center gap-3 text-sm text-slate-400">
                <span className={`rounded-full border px-2 py-1 text-[10px] uppercase tracking-[0.12em] ${audit.severity === 'Important' ? 'border-amber-500/30 bg-amber-500/10 text-amber-200' : 'border-slate-700 bg-slate-900 text-slate-300'}`}>
                  {audit.severity}
                </span>
                <span>{audit.user}</span>
                <span>{audit.timestamp}</span>
              </div>
            </div>
          ))}
        </div>
      </div>

      <div className="rounded-2xl border border-amber-500/30 bg-amber-500/10 p-4 text-sm text-amber-100">
        <div className="flex items-center gap-2 font-medium">
          <TriangleAlert className="h-4 w-4" />
          Security alert
        </div>
        <p className="mt-2">Three maintenance approvals were reviewed without a related downtime event. Follow-up recommended.</p>
      </div>
    </div>
  );
}
