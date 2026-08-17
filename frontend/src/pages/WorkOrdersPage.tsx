import { ClipboardList, Search, ShieldCheck } from 'lucide-react';

const workOrders = [
  { id: 101, title: 'Emergency inspection', machine: 'CNC-01', status: 'Requested', priority: 'High', due: '2 days' },
  { id: 102, title: 'Valve calibration', machine: 'BRL-01', status: 'Assigned', priority: 'Critical', due: '1 day' },
  { id: 103, title: 'Conveyor drive check', machine: 'ROB-01', status: 'In Progress', priority: 'Medium', due: '4 days' },
];

export default function WorkOrdersPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Maintenance workflow</p>
          <h1 className="text-3xl font-semibold text-white">Work Orders</h1>
        </div>
        <div className="flex items-center gap-3">
          <div className="flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-slate-300">
            <Search className="h-4 w-4 text-slate-500" />
            <input className="w-44 bg-transparent outline-none placeholder:text-slate-500" placeholder="Search WOs" />
          </div>
          <button className="rounded-xl bg-cyan-500 px-4 py-2 text-sm font-semibold text-slate-950">New work order</button>
        </div>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        {workOrders.map((workOrder) => (
          <div key={workOrder.id} className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-2 text-cyan-300">
                <ClipboardList className="h-4 w-4" />
                <span className="text-xs uppercase tracking-[0.18em]">WO-{workOrder.id}</span>
              </div>
              <div className="rounded-full border border-slate-700 bg-slate-950 px-2 py-1 text-[10px] uppercase tracking-wide text-slate-300">
                {workOrder.priority}
              </div>
            </div>

            <h2 className="text-lg font-semibold text-white">{workOrder.title}</h2>
            <p className="mt-2 text-sm text-slate-400">{workOrder.machine}</p>

            <div className="mt-4 flex items-center justify-between text-sm">
              <span className="text-slate-400">Status</span>
              <span className="font-medium text-emerald-300">{workOrder.status}</span>
            </div>
            <div className="mt-2 flex items-center justify-between text-sm">
              <span className="text-slate-400">Due</span>
              <span className="font-medium text-white">{workOrder.due}</span>
            </div>

            <button className="mt-5 inline-flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm font-medium text-white hover:border-cyan-500/40">
              <ShieldCheck className="h-4 w-4 text-cyan-300" />
              Review
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
