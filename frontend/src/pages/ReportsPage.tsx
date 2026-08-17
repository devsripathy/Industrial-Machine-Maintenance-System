export default function ReportsPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Analytics</p>
          <h1 className="text-3xl font-semibold text-white">Reports</h1>
        </div>
        <button className="rounded-xl border border-slate-700 bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:border-cyan-500/40">
          Download PDF
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="text-sm text-slate-400">Availability</div>
          <div className="mt-3 text-3xl font-semibold text-white">98.7%</div>
          <p className="mt-2 text-xs text-emerald-300">+1.4% vs last week</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="text-sm text-slate-400">MTBF</div>
          <div className="mt-3 text-3xl font-semibold text-white">520h</div>
          <p className="mt-2 text-xs text-cyan-300">3.1% above target</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <div className="text-sm text-slate-400">MTTR</div>
          <div className="mt-3 text-3xl font-semibold text-white">7.2h</div>
          <p className="mt-2 text-xs text-violet-300">11% below benchmark</p>
        </div>
      </div>
    </div>
  );
}
