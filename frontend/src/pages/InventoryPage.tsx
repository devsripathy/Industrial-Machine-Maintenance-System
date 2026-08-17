import { PackageSearch, TrendingDown } from 'lucide-react';

const inventory = [
  { name: 'Hydraulic seal kit', sku: 'HSK-220', stock: 28, min: 12, status: 'Healthy' },
  { name: 'Drive belt', sku: 'DB-195', stock: 7, min: 14, status: 'Reorder' },
  { name: 'Temperature sensor', sku: 'TS-406', stock: 18, min: 10, status: 'Healthy' },
];

export default function InventoryPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Inventory</p>
          <h1 className="text-3xl font-semibold text-white">Spare parts</h1>
        </div>
        <button className="rounded-xl bg-cyan-500 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-cyan-400">
          Create stock request
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Parts on hand</p>
          <div className="mt-3 text-3xl font-semibold text-white">1,248</div>
          <p className="mt-1 text-sm text-slate-400">Across 42 SKUs</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Low stock</p>
          <div className="mt-3 text-3xl font-semibold text-amber-300">07</div>
          <p className="mt-1 text-sm text-slate-400">Reorder recommended</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Request cycle</p>
          <div className="mt-3 text-3xl font-semibold text-emerald-300">3.2d</div>
          <p className="mt-1 text-sm text-slate-400">Average replenishment time</p>
        </div>
      </div>

      <div className="grid gap-4 lg:grid-cols-3">
        {inventory.map((item) => (
          <div key={item.sku} className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-2 text-cyan-300">
                <PackageSearch className="h-4 w-4" />
                <span className="text-xs uppercase tracking-[0.18em]">{item.sku}</span>
              </div>
              <div className={`rounded-full px-2 py-1 text-[10px] uppercase tracking-[0.12em] ${item.status === 'Reorder' ? 'bg-amber-500/10 text-amber-200 border border-amber-500/30' : 'bg-emerald-500/10 text-emerald-200 border border-emerald-500/30'}`}>
                {item.status}
              </div>
            </div>

            <h2 className="text-lg font-semibold text-white">{item.name}</h2>
            <div className="mt-4 space-y-2 text-sm text-slate-300">
              <div className="flex items-center justify-between">
                <span className="text-slate-400">On hand</span>
                <span>{item.stock}</span>
              </div>
              <div className="flex items-center justify-between">
                <span className="text-slate-400">Minimum</span>
                <span>{item.min}</span>
              </div>
            </div>

            {item.status === 'Reorder' && (
              <button className="mt-5 inline-flex items-center gap-2 rounded-xl border border-amber-500/40 bg-amber-500/10 px-3 py-2 text-sm font-medium text-amber-200">
                <TrendingDown className="h-4 w-4" />
                Reorder
              </button>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
